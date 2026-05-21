# coding: utf-8
import sys
sys.stdout.reconfigure(encoding='utf-8')

data = open(r'C:\Users\Public\Card3500_unpacked.exe', 'rb').read()
mpos = 4427272
window_bytes = data[mpos:mpos+60000]
window_latin1 = window_bytes.decode('latin1')
cmdexit_idx = window_latin1.find('CmdExit')
segment = window_bytes[cmdexit_idx:cmdexit_idx+300]
caption_utf16 = 'caption'.encode('utf-16-le')
cap_idx = segment.find(caption_utf16)
print('caption at +%d' % cap_idx)
after = segment[cap_idx + len(caption_utf16):]
print('Bytes after caption (40):', after[:40].hex())
print('Type byte: 0x%02x' % after[0])

# Length at bytes 1-4 (little endian)
length = int.from_bytes(after[1:5], 'little')
print('4-byte length = %d' % length)
if 0 < length < 20:
    val = after[5:5+length*2].decode('utf-16-le', 'replace')
    print('Caption (widestring): %s' % val)

# Also check cmdSave caption  
cmdsave_idx = window_latin1.find('cmdSave')
print('\ncmdSave at offset %d' % cmdsave_idx)
seg2 = window_bytes[cmdsave_idx:cmdsave_idx+300]
cap_idx2 = seg2.find(caption_utf16)
if cap_idx2 >= 0:
    after2 = seg2[cap_idx2 + len(caption_utf16):]
    print('cmdSave caption bytes:', after2[:20].hex())
    length2 = int.from_bytes(after2[1:5], 'little')
    print('4-byte length = %d' % length2)
    if 0 < length2 < 20:
        val2 = after2[5:5+length2*2].decode('utf-16-le', 'replace')
        print('cmdSave Caption: %s' % val2)
