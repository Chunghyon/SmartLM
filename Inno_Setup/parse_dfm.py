# coding: utf-8
import sys
sys.stdout.reconfigure(encoding='utf-8')

data = open(r'C:\Users\Public\Card3500_unpacked.exe', 'rb').read()
mpos = 4427272
window_bytes = data[mpos:mpos+60000]
window_latin1 = window_bytes.decode('latin1')

# After class name 'AnoleCommandButton', the first property
# From hex: 2e6163427574746f6e00 = ".acButton\0" (parent class)
# then: 03 c8 0a f8 07 47 04 77 01 0f 03 00 2d 4c 42 09 00 ...
# Delphi DFM binary format after class:
# Properties are:
#   - name as Pascal short string (length byte + chars)
#   - then type byte + value
# Followed by 0x00 to end properties, then sub-objects, then 0x00 to end object

# After ".acButton\0" for AnoleCommandButton:
# 03 = "Left" (length=3) + "Lef"? No...
# Let me decode more carefully

# Actually AnoleCommandButton inherits, let's look at its DFM structure more carefully
cmdsave_idx = window_latin1.find('cmdSave')
seg = window_bytes[cmdsave_idx:cmdsave_idx+600]

# Find the property block
anole_idx = seg.find(b'AnoleCommandButton')
class_end = anole_idx + len(b'AnoleCommandButton')
print('Properties start at +%d' % class_end)

# Read properties
i = class_end
count = 0
while i < len(seg) - 1 and count < 30:
    if seg[i] == 0:
        print('End of properties at +%d' % i)
        break
    name_len = seg[i]
    if name_len > 50 or name_len == 0:
        print('Unexpected name_len=%d at +%d, stopping' % (name_len, i))
        break
    name = seg[i+1:i+1+name_len].decode('latin1', 'replace')
    i += 1 + name_len
    if i >= len(seg):
        break
    type_tag = seg[i]
    i += 1
    print('Property: %s, type=0x%02x' % (name, type_tag))

    # Read value based on type
    if type_tag == 0x03:  # vaInt8
        val = seg[i]
        i += 1
        print('  value(int8):', val)
    elif type_tag == 0x04:  # vaInt16
        val = int.from_bytes(seg[i:i+2], 'little', signed=True)
        i += 2
        print('  value(int16):', val)
    elif type_tag == 0x05:  # vaInt32
        val = int.from_bytes(seg[i:i+4], 'little', signed=True)
        i += 4
        print('  value(int32):', val)
    elif type_tag == 0x07:  # vaString (Pascal short string with 1-byte length)
        slen = seg[i]
        i += 1
        val = seg[i:i+slen].decode('gbk', 'replace')
        i += slen
        print('  value(string):', repr(val))
    elif type_tag == 0x0A:  # vaLString (longstring with 4-byte length)
        slen = int.from_bytes(seg[i:i+4], 'little')
        i += 4
        val = seg[i:i+slen].decode('gbk', 'replace')
        i += slen
        print('  value(lstring):', repr(val))
    elif type_tag == 0x10:  # vaWString
        wlen = int.from_bytes(seg[i:i+4], 'little')
        i += 4
        val = seg[i:i+wlen*2].decode('utf-16-le', 'replace')
        i += wlen*2
        print('  value(wstring):', repr(val))
    elif type_tag == 0x0B:  # vaFalse
        print('  value: False')
    elif type_tag == 0x0C:  # vaTrue
        print('  value: True')
    elif type_tag == 0x0D:  # vaIdent
        slen = seg[i]
        i += 1
        val = seg[i:i+slen].decode('latin1', 'replace')
        i += slen
        print('  value(ident):', val)
    elif type_tag == 0x0F:  # vaChar
        val = seg[i]
        i += 1
        print('  value(char):', chr(val))
    else:
        print('  unknown type, stopping')
        break
    count += 1
