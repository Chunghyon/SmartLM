# coding: utf-8
# REPLACED
import sys
sys.stdout.reconfigure(encoding='utf-8')

data = open(r'C:\Users\Public\Card3500_unpacked.exe', 'rb').read()
mpos = 4427272
window_bytes = data[mpos:mpos+60000]
window_latin1 = window_bytes.decode('latin1')

# Anole DFM is NOT standard Delphi DFM
# It's a custom binary format for Anole controls
# The caption is stored differently

# Let's approach from a different angle:
# Find the two buttons in ImageMinTip form and their captions 
# by looking at UTF-16 strings near CmdExit and cmdSave

# First, let's check what the exe stores as UTF-16 captions for known buttons
# Then find CmdExit's caption

# Look for GBK Chinese text in the area around form definitions
# The form is loaded as a resource and Anole reads captions from it

# Different approach: search for known GBK strings in the exe
# The Chinese caption GBK bytes should be somewhere

# Screen shows: button1='?зи' (GBK c0fa c0e5) button2='??' (GBK b8d9 b7e1)
# Wait - let me recalculate 
# '?' Unicode = U+5386, GBK = ?
import struct

ch1 = '?'
ch2 = 'зи'
ch3 = '?'
ch4 = '?'
print('? GBK:', ''.join('%02X' % b for b in ch1.encode('gbk')))
print('зи GBK:', ''.join('%02X' % b for b in ch2.encode('gbk')))
print('? GBK:', ''.join('%02X' % b for b in ch3.encode('gbk')))
print('? GBK:', ''.join('%02X' % b for b in ch4.encode('gbk')))

# So screen bytes as GBK:
# button1: '?зи' = C0FA C0E5 
# button2: '??' = B8D9 B7E1

# These bytes as EUC-KR (CP949):
b1 = bytes([0xC0, 0xFA, 0xC0, 0xE5])
b2 = bytes([0xB8, 0xD9, 0xB7, 0xE1])
print()
print('C0FAC0E5 as CP949:', b1.decode('cp949', 'replace'))
print('B8D9B7E1 as CP949:', b2.decode('cp949', 'replace'))

# So exe is running as CP949 (Korean), and stores:
# button1 caption = bytes C0 FA C0 E5 = 'РњРх' in CP949
# button2 caption = bytes B8 D9 B7 E1 = ?? in CP949

# But screen shows GBK interpretation
# This means exe uses GBK for display even though system is Korean
# This doesn't make sense

# Alternative: exe runs as GBK system, buttons show GBK chars
# User's system is set to Chinese locale for this app?

# Actually - the screen shows EXACTLY what's in the exe binary as GBK
# So the exe stores captions in GBK
# button1 = b8d9 b7e1 GBK = '??'? 
# Wait, I had it backwards. Let me re-read the screenshot description.
# Screenshot shows: first button '?зи', second '??'
# ?зи is the FIRST button

# So first button = '?зи' (GBK) = C0FA C0E5 
# But C0FA C0E5 as CP949 = 'РњРх' (save)
# Second button = '??' (GBK) 
print()
print('? as CP949:', bytes([0xB8, 0xD9]).decode('cp949', 'replace'))
print('? as CP949:', bytes([0xB7, 0xE1]).decode('cp949', 'replace'))
# ? GBK = b8d9, ? GBK = b7e1
# B8D9 CP949 = ?
# B7E1 CP949 = ?

# Search for these bytes in exe
patterns = [
    (b'\xc0\xfa\xc0\xe5', '?зи'),
    (b'\xb8\xd9\xb7\xe1', '??'),
]
for pat, name in patterns:
    positions = []
    i = 0
    while True:
        idx = data.find(pat, i)
        if idx < 0: break
        positions.append(idx)
        i = idx + 1
    print('Pattern %s (%s): found at %d positions' % (name, pat.hex(), len(positions)))
    for p in positions[:5]:
        ctx = data[max(0,p-10):p+20].decode('latin1')
        printable = ''.join(c if 32 <= ord(c) <= 126 else '.' for c in ctx)
        print('  pos=%d: %s' % (p, printable))
