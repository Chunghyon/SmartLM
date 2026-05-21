# coding: utf-8
import sys
sys.stdout.reconfigure(encoding='utf-8')

data = open(r'C:\Users\Public\Card3500_unpacked.exe', 'rb').read()
latin1 = data.decode('latin1')

# ImageMinTip form start
mpos = 4427272
window_bytes = data[mpos:mpos+60000]
window_latin1 = window_bytes.decode('latin1')

# Find CmdExit control
cmdexit_idx = window_latin1.find('CmdExit')
print(f'CmdExit at offset {cmdexit_idx} (abs {mpos+cmdexit_idx})')

# After CmdExit, find the caption value
# In VB6/Delphi DFM, properties like "caption" are stored as wide strings
# The format is: property_name(wide) + type_byte + value
# For WideStrings, after "caption" comes 06 (type) + length(4 bytes) + chars

segment = window_bytes[cmdexit_idx:cmdexit_idx+600]

# Find 'caption' as UTF-16LE
caption_utf16 = 'caption'.encode('utf-16-le')
for i in range(len(segment) - len(caption_utf16)):
    if segment[i:i+len(caption_utf16)] == caption_utf16:
        print(f'caption UTF-16 at +{i}')
        after = segment[i+len(caption_utf16):]
        print(f'Next 30 bytes: {after[:30].hex()}')
        # Try to read the caption value
        # Type byte 06 = WideString
        if after[0] == 0x06:
            val_len = int.from_bytes(after[1:5], 'little')
            print(f'WideString len: {val_len} chars')
            val = after[5:5+val_len*2].decode('utf-16-le', 'replace')
            print(f'Caption: {val}')
        break

# Also check: what's between CmdExit and end of form
# Find all GBK/EUC-KR 2-byte sequences near caption
for i in range(len(segment)-1):
    b1, b2 = segment[i], segment[i+1]
    if 0x81 <= b1 <= 0xFE and 0x40 <= b2 <= 0xFE:
        try:
            ch = bytes([b1, b2]).decode('gbk')
            if '\u4e00' <= ch <= '\u9fff' or '\u3400' <= ch <= '\u4dbf':
                print(f'GBK Chinese at +{i}: {ch} ({b1:02X}{b2:02X})')
        except:
            pass
