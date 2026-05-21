import os, re, shutil, sys
sys.stdout.reconfigure(encoding='utf-8')

APP = r'C:\Program Files (x86)\Access Control System'
INNO = r'D:\Documents\Smart_LM_China\Inno_Setup'

# 1. Korean.XML
korean_fixes = [
    ('\u7b7e\u540d', '\uc11c\uba85', 'RecordSignature'),
    ('\u672a\u7b7e\u540d', '\ubbf8\uc11c\uba85', 'RecordSignature_0'),
    ('\u5df2\u7b7e\u540d', '\uc11c\uba85\ub428', 'RecordSignature_1'),
    ('\u5df2\u9a8c\u8bc1', '\uac80\uc99d\ub428', 'RecordSignature_2'),
    ('\u9a8c\u8bc1\u9519\u8bef', '\uac80\uc99d \uc624\ub958', 'RecordSignature_3'),
    ('\u6253\u5370\u51fa\u5165\u8bb0\u5f55\u62a5\u8868', '\uc785\ucd9c \uae30\ub85d \ubcf4\uace0\uc11c \uc778\uc1c4', 'PrintRecord'),
    ('\u5bfc\u51fa\u51fa\u5165\u8bb0\u5f55\u62a5\u8868', '\uc785\ucd9c \uae30\ub85d \ubcf4\uace0\uc11c \ub0b4\ubcf4\ub0b4\uae30', 'OutputRecord'),
    ('\u5bfc\u51fa\u51fa\u5165\u8bb0\u5f55\u7edf\u8ba1\u62a5\u8868', '\uc785\ucd9c \uae30\ub85d \ud1b5\uacc4 \ubcf4\uace0\uc11c \ub0b4\ubcf4\ub0b4\uae30', 'OutputTotalRecord'),
    ('\u6253\u5370\u51fa\u5165\u8bb0\u5f55\u7167\u7247\u62a5\u8868', '\uc785\ucd9c \uae30\ub85d \uc0ac\uc9c4 \ubcf4\uace0\uc11c \uc778\uc1c4', 'PrintFaceRecord'),
    ('\u5bfc\u51fa\u51fa\u5165\u8bb0\u5f55\u7167\u7247\u62a5\u8868', '\uc785\ucd9c \uae30\ub85d \uc0ac\uc9c4 \ubcf4\uace0\uc11c \ub0b4\ubcf4\ub0b4\uae30', 'OutputFaceRecord'),
    ('\u67e5\u8be2\u51fa\u5165\u8bb0\u5f55\u62a5\u8868', '\uc785\ucd9c \uae30\ub85d \ubcf4\uace0\uc11c \uc870\ud68c', 'SearchRecordLog'),
    ('\u4e8b\u4ef6\uff1a', '\uc774\ubca4\ud2b8:', 'lblEventCode'),
    ('\u652f\u6301\u591a\u6761\u4ef6\u8f93\u5165\uff0c\u4f7f\u7528\u9017\u53f7\u5206\u9694\uff0c\u793a\u4f8b\uff1a \u5237\u5361\u9a8c\u8bc1,\u4eba\u8138\u9a8c\u8bc1',
     '\uc5ec\ub7ec \uc870\uac74 \uc785\ub825 \uc9c0\uc6d0, \uc27c\ud45c\ub85c \uad6c\ubd84. \uc608: \uce74\ub4dc \uc778\uc99d, \uc5bc\uad74 \uc778\uc99d', 'cmbEventCodeToolTip'),
    ('\u4fee\u6539\u4eba\u5458\u6743\u9650\u7ec4: %PName (%PCode)\uff1b \u65b0\u6743\u9650\u7ec4:%Group.',
     '\uc0ac\uc6a9\uc790 \ucd9c\uc785 \uadf8\ub8f9 \ubcc0\uacbd: %PName (%PCode); \uc0c8 \uadf8\ub8f9: %Group.', 'UpdateEmplAccessGroupLog'),
    ('\u5220\u9664\u4eba\u5458\u6743\u9650\u7ec4: %PName (%PCode)\uff1b\u6240\u5c5e\u6743\u9650\u7ec4:%Group.',
     '\uc0ac\uc6a9\uc790 \ucd9c\uc785 \uadf8\ub8f9 \uc0ad\uc81c: %PName (%PCode); \uc18c\uc18d \uadf8\ub8f9: %Group.', 'DeleteEmplAccessGroupLog'),
]

for fpath in [os.path.join(APP, 'Korean.XML'), os.path.join(INNO, 'Korean.XML')]:
    c = open(fpath, encoding='utf-8').read()
    n = 0
    for zh, kr, tag in korean_fixes:
        old = f'<{tag}>{zh}</{tag}>'
        new = f'<{tag}>{kr}</{tag}>'
        if old in c:
            c = c.replace(old, new); n += 1
    open(fpath, 'w', encoding='utf-8').write(c)
    print(f'Korean.XML: {n}')

# 2. SoftWareInfo_Korean.XML
equpt = [
    ('EquptTypeInfo_E32', '32\ucc44\ub110 %1 \ucf58\ud2b8\ub864\ub7ec. 32\uac1c %2 \uc9c0\uc6d0, \ud655\uc7a5 \ubcf4\ub4dc \ucd94\uac00 \uc2dc 64\uac1c %2\uae4c\uc9c0 \ud655\uc7a5 \uac00\ub2a5. \uba54\uc778\ubcf4\ub4dc \ucd5c\ub300 26,000\uba85 \uc0ac\uc6a9\uc790, 100,000\uac74 \uae30\ub85d \uc9c0\uc6d0.'),
    ('EquptTypeInfo_E16', '16\ucc44\ub110 %1 \ucf58\ud2b8\ub864\ub7ec. 16\uac1c %2 \uc9c0\uc6d0, \ud655\uc7a5 \ubcf4\ub4dc \ucd94\uac00 \uc2dc 32\uac1c %2\uae4c\uc9c0 \ud655\uc7a5 \uac00\ub2a5. \uba54\uc778\ubcf4\ub4dc \ucd5c\ub300 26,000\uba85 \uc0ac\uc6a9\uc790, 100,000\uac74 \uae30\ub85d \uc9c0\uc6d0.'),
    ('EquptTypeInfo_OE8', '\uc624\ud504\ub77c\uc778 8\ucc44\ub110 %1 \ucf58\ud2b8\ub864\ub7ec. 8\uac1c %2 \uc9c0\uc6d0, \ud655\uc7a5 \ubcf4\ub4dc \ucd94\uac00 \uc2dc 16\uac1c %2\uae4c\uc9c0 \ud655\uc7a5 \uac00\ub2a5. \uba54\uc778\ubcf4\ub4dc \ucd5c\ub300 90,000\uba85 \uc0ac\uc6a9\uc790, 100,000\uac74 \uae30\ub85d \uc9c0\uc6d0.'),
    ('EquptTypeInfo_OE16', '\uc624\ud504\ub77c\uc778 16\ucc44\ub110 %1 \ucf58\ud2b8\ub864\ub7ec. 16\uac1c %2 \uc9c0\uc6d0. \uba54\uc778\ubcf4\ub4dc \ucd5c\ub300 90,000\uba85 \uc0ac\uc6a9\uc790, 100,000\uac74 \uae30\ub85d \uc9c0\uc6d0.'),
]

for fpath in [os.path.join(APP, 'SoftWareInfo_Korean.XML'), os.path.join(INNO, 'SoftWareInfo_Korean.XML')]:
    c = open(fpath, encoding='utf-8').read()
    n = 0
    for tag, kr in equpt:
        c, cnt = re.subn(f'<{tag}>[^<]*</{tag}>', f'<{tag}>{kr}</{tag}>', c)
        n += cnt
    open(fpath, 'w', encoding='utf-8').write(c)
    print(f'SoftWareInfo_Korean.XML: {n}')

# 3. ICCardEditer Korean.xml
lines = [
    '<?xml version="1.0" encoding="utf-8" ?>',
    '<Language>',
    '  <FrmLogin>',
    '    <FormCaption>IC\uce74\ub4dc/CPU\uce74\ub4dc \ud0a4 \ud3b8\uc9d1\uae30 ver1.9</FormCaption>',
    '    <Label4>IC\uce74\ub4dc/CPU\uce74\ub4dc \ud0a4 \ud3b8\uc9d1\uae30</Label4>',
    '    <Label1>\uc0ac\uc6a9\uc790\uba85\uff1a</Label1>',
    '    <Label2>\ube44\ubc00\ubc88\ud638\uff1a</Label2>',
    '    <ButLogin>\ub85c\uadf8\uc778</ButLogin>',
    '    <ButExit>\uc885\ub8cc</ButExit>',
    '    <MsgBox1>\ub370\uc774\ud130\ubca0\uc774\uc2a4 \uc5f0\uacb0 \uc911, \uc7a0\uc2dc \uae30\ub2e4\ub824 \uc8fc\uc138\uc694....</MsgBox1>',
    '    <MsgBox2>\ub370\uc774\ud130 \ud30c\uc77c\uc774 \uc5c6\uc2b5\ub2c8\ub2e4</MsgBox2>',
    '    <MsgBox3>\ub370\uc774\ud130\ubca0\uc774\uc2a4 \uc5f0\uacb0 \uc131\uacf5. \ube44\ubc00\ubc88\ud638\ub97c \uc785\ub825\ud558\uc138\uc694!</MsgBox3>',
    '    <MsgBox4>\ub370\uc774\ud130\ubca0\uc774\uc2a4 \uc5f0\uacb0 \uc2e4\ud328!</MsgBox4>',
    '    <MsgBox5>\uce74\ub4dc \uc18c\ud504\ud2b8\uc6e8\uc5b4\ub97c \ud1b5\ud574 \ub370\uc774\ud130\ubca0\uc774\uc2a4 \uc5f0\uacb0 \ud30c\ub77c\ubbf8\ud130\ub97c \ud655\uc778\ud574 \uc8fc\uc138\uc694!</MsgBox5>',
    '    <MsgBox6>\uad00\ub9ac\uc790 \ube44\ubc00\ubc88\ud638\ub97c \uc785\ub825\ud558\uc138\uc694!</MsgBox6>',
    '    <MsgBox7>\uad00\ub9ac\uc790 \ube44\ubc00\ubc88\ud638 \uc624\ub958!</MsgBox7>',
    '  </FrmLogin>',
    '  <FrmEdit>',
    '    <FormCaption>IC\uce74\ub4dc/CPU\uce74\ub4dc \ud0a4 \ud3b8\uc9d1\uae30 ver1.9</FormCaption>',
    '    <stbICPassword>\uce74\ub4dc \ube44\ubc00\ubc88\ud638 \ud45c</stbICPassword>',
    '    <stbEncrypt>\uc554\ud638\ud654/\ubcf5\ud638\ud654</stbEncrypt>',
    '    <SuperTabItem1>\uce74\ub4dc \ub9ac\ub354\uae30 \uc124\uc815</SuperTabItem1>',
    '    <LabelX3>\uce74\ub4dc \ubc1c\uae09 \ub610\ub294 \uc18c\ube44 \uc18c\ud504\ud2b8\uc6e8\uc5b4 \ubc1c\uae09 \ud6c4\uc5d0\ub294 \ubcc0\uacbd\ud560 \uc218 \uc5c6\uc2b5\ub2c8\ub2e4</LabelX3>',
    '    <butSave>\uc800\uc7a5</butSave>',
    '    <butExit>\uc885\ub8cc</butExit>',
    '    <LblSecretkey>2\ucc28 \uc554\ud638\ud654 \ube44\ubc00\ubc88\ud638 (\ubcf5\ud638\ud654\uc5d0 \uc0ac\uc6a9):</LblSecretkey>',
    '    <LabelX1>\uce74\ub4dc \ubc1c\uae09\uae30 \uc720\ud615\uff1a</LabelX1>',
    '    <LabelX2>\uc0c1\ud0dc\uff1a</LabelX2>',
    '    <LabelX5>\uc554\ud638\ud654 \ud69f\uc218\uff1a</LabelX5>',
    '    <ButEncode>\uc554\ud638\ud654</ButEncode>',
    '    <butDecode>\ubcf5\ud638\ud654</butDecode>',
    '    <butExit1>\uc885\ub8cc</butExit1>',
    '    <butStop>\uc911\uc9c0</butStop>',
    '    <Label15>\uce74\ub4dc \ub9ac\ub354\uae30 \ubaa8\ub4c8\uc774 \uc9c0\uc6d0\ud558\ub294 \uce74\ub4dc \uc720\ud615\uff1a</Label15>',
    '    <Label7>\ucf58\ud2b8\ub864 \uce74\ub4dc \uae30\ub2a5 \uc2a4\uc704\uce58\uff1a</Label7>',
    '    <btnWriteConfigCard>\ucf58\ud2b8\ub864 \uce74\ub4dc \uae30\ub2a5 \uc2a4\uc704\uce58 \uc124\uc815 \uce74\ub4dc \ub9cc\ub4e4\uae30</btnWriteConfigCard>',
    '    <NotReader>\uce74\ub4dc \ub9ac\ub354\uae30\ub97c \ucc3e\uc744 \uc218 \uc5c6\uc2b5\ub2c8\ub2e4</NotReader>',
    '    <ParReader>\ud30c\ub77c\ubbf8\ud130 \uc624\ub958</ParReader>',
    '    <Password>\ube44\ubc00\ubc88\ud638 \uc624\ub958</Password>',
    '    <NotCard>\uce74\ub4dc\uac00 \uc5c6\uc2b5\ub2c8\ub2e4</NotCard>',
    '    <DataErr>\ub370\uc774\ud130 \ube14\ub85d \ud06c\uae30 \uc624\ub958</DataErr>',
    '    <Sections>\uc139\ud130,\ube44\ubc00\ubc88\ud638,\uc0ac\uc6a9,\uc6a9\ub3c4</Sections>',
    '    <ReaderTypes>MF1 IC\uce74\ub4dc,NFC\uce74\ub4dc,NFC\ud3f0,\uc2e0\ubd84\uc99d,CPU IC\uce74\ub4dc,CPU\uce74\ub4dc,ID\uce74\ub4dc</ReaderTypes>',
    '    <SectionTypes0>\ubbf8\uc0ac\uc6a9</SectionTypes0>',
    '    <SectionTypes1>\ubc94\uc6a9 \uce74\ub4dc \ubc88\ud638</SectionTypes1>',
    '    <SectionTypes2>Wiegand \ub9ac\ub354\uae30</SectionTypes2>',
    '    <SectionTypes3>\uadfc\ud0dc \uc2dc\uc2a4\ud15c</SectionTypes3>',
    '    <SectionTypes4>\uc2a4\ub9c8\ud2b8 \uc804\ub825\uacc4</SectionTypes4>',
    '    <SectionTypes5>\uad50\ud1b5 \uc2dc\uc2a4\ud15c1</SectionTypes5>',
    '    <SectionTypes6>\uad50\ud1b5 \uc2dc\uc2a4\ud15c2</SectionTypes6>',
    '    <SectionTypes7>\uc624\ud504\ub77c\uc778 \ucd9c\uc785\ud1b5\uc81c/\uc778\ud130\ud3f0 \uc2dc\uc2a4\ud15c</SectionTypes7>',
    '    <SectionTypes8>\uc624\ud504\ub77c\uc778 \uc5d8\ub9ac\ubca0\uc774\ud130 \uc81c\uc5b4</SectionTypes8>',
    '    <SectionTypes9>\uc18c\ube44 \uc2dc\uc2a4\ud15c1</SectionTypes9>',
    '    <SectionTypes10>\uc18c\ube44 \uc2dc\uc2a4\ud15c2</SectionTypes10>',
    '    <SectionTypes11>\uc18c\ube44 \uc2dc\uc2a4\ud15c3</SectionTypes11>',
    '    <SectionTypes12>\ubb3c \uad00\ub9ac \uc2dc\uc2a4\ud15c</SectionTypes12>',
    '    <SectionTypes13>\ud638\ud154 \uc7a0\uae08/\uac00\uc815\uc6a9 \uc7a0\uae08</SectionTypes13>',
    '    <SectionTypes14>\uc5b4\ub9b0\uc774\uc9d1 \uc2dc\uc2a4\ud15c</SectionTypes14>',
    '    <SectionTypes15>\uc8fc\ucc28\uc7a5 \uc2dc\uc2a4\ud15c</SectionTypes15>',
    '    <SectionTypes16>\uc2dc\uc2a4\ud15c \ud14c\uc2a4\ud2b8</SectionTypes16>',
    '    <MsgBox1>\uba3c\uc800 \uc554\ud638\ud654/\ubcf5\ud638\ud654\ub97c \uc911\uc9c0\ud558\uc138\uc694!</MsgBox1>',
    '    <MsgBox2>\ube44\ubc00\ubc88\ud638 \ud45c\uac00 \uc218\uc815\ub418\uc5c8\uc73c\ub098 \uc800\uc7a5\ub418\uc9c0 \uc54a\uc558\uc2b5\ub2c8\ub2e4. \uc9c0\uae08 \uc800\uc7a5\ud558\uc2dc\uaca0\uc2b5\ub2c8\uae4c?</MsgBox2>',
    '    <MsgBox3>\ud655\uc778</MsgBox3>',
    '    <MsgBox4>\uc2dc\uc2a4\ud15c \uc554\ud638\ud654 \ud30c\uc77c\uc774 \uc5c6\uc2b5\ub2c8\ub2e4!</MsgBox4>',
    '    <MsgBox5>\uc624\ub958</MsgBox5>',
    '    <MsgBox6>\uc139\ud130 \ube44\ubc00\ubc88\ud638\ub294 12\uc790\ub9ac\uc5ec\uc57c \ud569\ub2c8\ub2e4. \uc0ac\uc6a9 \uac00\ub2a5 \ubb38\uc790: 1234567890ABCDEF!</MsgBox6>',
    '    <MsgBox7>\uc54c\ub9bc</MsgBox7>',
    '    <MsgBox8>\uc139\ud130 \ube44\ubc00\ubc88\ud638\ub294 12\uc790\ub9ac\uc5ec\uc57c \ud569\ub2c8\ub2e4!</MsgBox8>',
    '    <MsgBox9>\uc800\uc7a5 \uc644\ub8cc!</MsgBox9>',
    '    <MsgBox10>\ube44\ubc00\ubc88\ud638 \ud45c\uc758 \uc139\ud130 {0} \ube44\ubc00\ubc88\ud638\uac00 \uc62c\ubc14\ub974\uc9c0 \uc54a\uc2b5\ub2c8\ub2e4. 12\uc790\ub9ac \ube44\ubc00\ubc88\ud638\ub97c \uc785\ub825\ud558\uc138\uc694!</MsgBox10>',
    '    <MsgBox11>\ube44\ubc00\ubc88\ud638 \ud45c\uc758 \uc139\ud130 {0} \uc6a9\ub3c4\uac00 \ub2e4\ub978 \uc139\ud130\uc640 \uc911\ubcf5\ub429\ub2c8\ub2e4. \uc218\uc815\ud574 \uc8fc\uc138\uc694!</MsgBox11>',
    '    <MsgBox12>\uce74\ub4dc \ub9ac\ub354\uae30\ub97c \ucc3e\uc744 \uc218 \uc5c6\uc2b5\ub2c8\ub2e4</MsgBox12>',
    '    <MsgBox13>\uce74\ub4dc \uac10\uc9c0\ub428, {0} \ucc98\ub9ac \uc911...</MsgBox13>',
    '    <MsgBox14>\uc9c0\uc6d0\ud558\uc9c0 \uc54a\ub294 \uc720\ud615\uc785\ub2c8\ub2e4 --</MsgBox14>',
    '    <MsgBox15>\uce74\ub4dc\uac00 \uac10\uc9c0\ub418\uc9c0 \uc54a\uc2b5\ub2c8\ub2e4</MsgBox15>',
    '    <MsgBox16>\uce74\ub4dc \ub9ac\ub354\uae30 \ud655\uc778 \uc911....</MsgBox16>',
    '    <MsgBox17>\uc554\ud638\ud654 \uc131\uacf5</MsgBox17>',
    '    <MsgBox18>\uc554\ud638\ud654 \uc2e4\ud328</MsgBox18>',
    '    <MsgBox19>\ubcf5\ud638\ud654 \uc131\uacf5</MsgBox19>',
    '    <MsgBox20>\ubcf5\ud638\ud654 \uc2e4\ud328</MsgBox20>',
    '    <MsgBox21>\uce74\ub4dc \ub9ac\ub354\uae30 \uc720\ud615</MsgBox21>',
    '    <MsgBox22>\uce74\ub4dc \uac10\uc9c0\ub428, \uc124\uc815 \uc4f0\ub294 \uc911...</MsgBox22>',
    '    <MsgBox23>\uc124\uc815 \uc4f0\uae30 \uc644\ub8cc!</MsgBox23>',
    '    <MsgBox24>\uc124\uc815 \uc4f0\uae30 \uc2e4\ud328. \uce74\ub4dc\ub97c \uad50\uccb4\ud574 \uc8fc\uc138\uc694!</MsgBox24>',
    '  </FrmEdit>',
    '</Language>',
]
iccard_path = os.path.join(APP, 'Language', 'ICCardEditer', 'Korean.xml')
open(iccard_path, 'w', encoding='utf-8').write('\n'.join(lines) + '\n')
inno_dir = os.path.join(INNO, 'Language', 'ICCardEditer')
os.makedirs(inno_dir, exist_ok=True)
shutil.copy(iccard_path, os.path.join(inno_dir, 'Korean.xml'))
print('ICCardEditer Korean.xml: OK')

# 4. ICCard_Editer.exe.config
config_path = os.path.join(APP, 'ICCard_Editer.exe.config')
c = open(config_path, encoding='utf-8').read()
# Languages ��� �߰�
c = re.sub(r'<add key="Languages" value="[^"]*" />',
           '<add key="Languages" value="Korean,English,\u7b80\u4f53,\u7e41\u4f53" />', c)
# Korean ��� ��� �߰� (Language_English �տ�)
if 'Language_Korean' not in c:
    c = c.replace('<add key="Language_English"',
                  '<add key="Language_Korean" value="Language\\ICCardEditer\\Korean.xml" />\n\t\t<add key="Language_English"')
# �⺻ �� Korean���� ����
c = re.sub(r'<add key="ToolLanguage" value="[^"]*" />',
           '<add key="ToolLanguage" value="Korean" />', c)
open(config_path, 'w', encoding='utf-8').write(c)
print('ICCard_Editer.exe.config: OK')

# Inno_Setup���� config ����
shutil.copy(config_path, os.path.join(INNO, 'ICCard_Editer.exe.config'))
print('DONE')
