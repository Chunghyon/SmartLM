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
after = segment[cap_idx + len(caption_utf16):]

# Type byte 0x02 = in VB6/Anole DFM binary, this is a short integer
# But for caption this doesn't make sense
# Let's look at how cmdSave caption is stored - it should be GBK Chinese
# after: 02 00 00 00 7f 4f 28 75 ... 
# 0x4f7f = CJK char U+4F7F = 'че'? 
# 0x754f? = ?
# Wait - Anole DFM might store caption as: 
# byte 02 = type WideString with 2-byte length?
# 02 = 2 chars, then the chars

# For CmdExit: after = 02 00 00 00 00 90 fa 51
# If type=string (narrow), length at next byte...
# Let me try: 0x02 is NOT type byte but part of property record
# Actually in Delphi DFM binary, property values:
# After property name, comes: type_tag (1 byte) + value
# type_tag: 01=vaNull, 02=vaList, 03=vaInt8, 04=vaInt16, 05=vaInt32,
#           06=vaExtended, 07=vaString, 08=vaBinary, 09=vaSet, 0A=vaLString,
#           0B=vaFalse, 0C=vaTrue, 0D=vaIdent, 0E=vaNULL, 0F=vaChar,
#           10=vaWString, 11=vaInt64, 12=vaUTF8String

print('CmdExit caption type: 0x%02x' % after[0])
# 0x02 = vaList? That doesn't make sense for a caption
# Actually in Anole DFM it might differ

# Let me look at another known property to calibrate
# Find 'cmdSave' and read its full property block  
cmdsave_idx = window_latin1.find('cmdSave')
seg2 = window_bytes[cmdsave_idx:cmdsave_idx+400]
print('\ncmdSave segment hex (first 200 bytes after name):')
name_end = seg2.find(b'AnoleCommandButton')
if name_end >= 0:
    print('AnoleCommandButton at +%d' % name_end)
    prop_area = seg2[name_end+len('AnoleCommandButton'):]
    print('After class name (60 bytes):', prop_area[:60].hex())

# Hmm, let me approach differently
# The 'caption' WideString property is found at +196 in CmdExit segment
# bytes: 02 00 00 00 00 90 fa 51 00 00
# In Delphi: vaList=02, followed by properties until end marker
# OR: 0x02 after 'caption' means the caption value follows as a 2-char wide string
# where chars are stored as: char1_lo char1_hi char2_lo char2_hi
# But after[1:5] = 00 00 00 90 and after[5:9] = fa 51 00 00
# That gives U+0000 U+0000 then fa51?

# New theory: Anole DFM stores caption as narrow string (ANSI/CP936)
# Let's find narrow 'caption' string
caption_narrow = b'caption'
cap_narrow_idx = segment.find(caption_narrow)
if cap_narrow_idx >= 0:
    print('\nNarrow caption at +%d' % cap_narrow_idx)
    after_n = segment[cap_narrow_idx + len(caption_narrow):]
    print('After narrow caption (30 bytes):', after_n[:30].hex())

# Let me just dump both segments as GBK
print('\nCmdExit segment GBK decode:')
for i in range(0, min(200, len(segment)), 2):
    try:
        ch = segment[i:i+2].decode('gbk')
        if '\u4e00' <= ch <= '\u9fff':
            print('  +%d: %s (U+%04X)' % (i, ch, ord(ch)))
    except:
        pass

# cmdSave segment
seg_save = window_bytes[cmdsave_idx:cmdsave_idx+200]
print('\ncmdSave segment GBK decode:')
for i in range(0, min(200, len(seg_save)), 2):
    try:
        ch = seg_save[i:i+2].decode('gbk')
        if '\u4e00' <= ch <= '\u9fff':
            print('  +%d: %s (U+%04X)' % (i, ch, ord(ch)))
    except:
        pass
