# -*- coding: utf-8 -*-
file = r'D:\Documents\Smart_LM_China\Inno_Setup\Korean.XML'
with open(file, encoding='utf-8') as f:
    content = f.read()

t = [
    ('<RecordSignature>\u7b7e\u540d</RecordSignature>', '<RecordSignature>\uc11c\uba85</RecordSignature>'),
    ('<RecordSignature_0>\u672a\u7b7e\u540d</RecordSignature_0>', '<RecordSignature_0>\ubbf8\uc11c\uba85</RecordSignature_0>'),
    ('<RecordSignature_1>\u5df2\u7b7e\u540d</RecordSignature_1>', '<RecordSignature_1>\uc11c\uba85\ub428</RecordSignature_1>'),
    ('<RecordSignature_2>\u5df2\u9a8c\u8bc1</RecordSignature_2>', '<RecordSignature_2>\uac80\uc99d\ub428</RecordSignature_2>'),
    ('<RecordSignature_3>\u9a8c\u8bc1\u9519\u8bef</RecordSignature_3>', '<RecordSignature_3>\uac80\uc99d \uc624\ub958</RecordSignature_3>'),
    ('<PrintRecord>\u6253\u5370\u51fa\u5165\u8bb0\u5f55\u62a5\u8868</PrintRecord>', '<PrintRecord>\uc785\ucd9c \uae30\ub85d \ubcf4\uace0\uc11c \uc778\uc1c4</PrintRecord>'),
    ('<OutputRecord>\u5bfc\u51fa\u51fa\u5165\u8bb0\u5f55\u62a5\u8868</OutputRecord>', '<OutputRecord>\uc785\ucd9c \uae30\ub85d \ubcf4\uace0\uc11c \ub0b4\ubcf4\ub0b4\uae30</OutputRecord>'),
    ('<OutputTotalRecord>\u5bfc\u51fa\u51fa\u5165\u8bb0\u5f55\u7edf\u8ba1\u62a5\u8868</OutputTotalRecord>', '<OutputTotalRecord>\uc785\ucd9c \uae30\ub85d \ud1b5\uacc4 \ubcf4\uace0\uc11c \ub0b4\ubcf4\ub0b4\uae30</OutputTotalRecord>'),
    ('<PrintFaceRecord>\u6253\u5370\u51fa\u5165\u8bb0\u5f55\u7167\u7247\u62a5\u8868</PrintFaceRecord>', '<PrintFaceRecord>\uc785\ucd9c \uae30\ub85d \uc0ac\uc9c4 \ubcf4\uace0\uc11c \uc778\uc1c4</PrintFaceRecord>'),
    ('<OutputFaceRecord>\u5bfc\u51fa\u51fa\u5165\u8bb0\u5f55\u7167\u7247\u62a5\u8868</OutputFaceRecord>', '<OutputFaceRecord>\uc785\ucd9c \uae30\ub85d \uc0ac\uc9c4 \ubcf4\uace0\uc11c \ub0b4\ubcf4\ub0b4\uae30</OutputFaceRecord>'),
    ('<SearchRecordLog>\u67e5\u8be2\u51fa\u5165\u8bb0\u5f55\u62a5\u8868</SearchRecordLog>', '<SearchRecordLog>\uc785\ucd9c \uae30\ub85d \ubcf4\uace0\uc11c \uc870\ud68c</SearchRecordLog>'),
    ('<lblEventCode>\u4e8b\u4ef6\uff1a</lblEventCode>', '<lblEventCode>\uc774\ubca4\ud2b8:</lblEventCode>'),
    ('<cmbEventCodeToolTip>\u652f\u6301\u591a\u6761\u4ef6\u8f93\u5165\uff0c\u4f7f\u7528\u9017\u53f7\u5206\u9694\uff0c\u793a\u4f8b\uff1a \u5237\u5361\u9a8c\u8bc1,\u4eba\u8138\u9a8c\u8bc1</cmbEventCodeToolTip>', '<cmbEventCodeToolTip>\uc5ec\ub7ec \uc870\uac74 \uc785\ub825 \uc9c0\uc6d0, \uc27c\ud45c\ub85c \uad6c\ubd84. \uc608: \uce74\ub4dc \uc778\uc99d, \uc5bc\uad74 \uc778\uc99d</cmbEventCodeToolTip>'),
    ('<UpdateEmplAccessGroupLog>\u4fee\u6539\u4eba\u5458\u6743\u9650\u7ec4: %PName (%PCode)\uff1b \u65b0\u6743\u9650\u7ec4:%Group.</UpdateEmplAccessGroupLog>', '<UpdateEmplAccessGroupLog>\uc0ac\uc6a9\uc790 \ucd9c\uc785 \uadf8\ub8f9 \ubcc0\uacbd: %PName (%PCode); \uc0c8 \uadf8\ub8f9: %Group.</UpdateEmplAccessGroupLog>'),
    ('<DeleteEmplAccessGroupLog>\u5220\u9664\u4eba\u5458\u6743\u9650\u7ec4: %PName (%PCode)\uff1b\u6240\u5c5e\u6743\u9650\u7ec4:%Group.</DeleteEmplAccessGroupLog>', '<DeleteEmplAccessGroupLog>\uc0ac\uc6a9\uc790 \ucd9c\uc785 \uadf8\ub8f9 \uc0ad\uc81c: %PName (%PCode); \uc18c\uc18d \uadf8\ub8f9: %Group.</DeleteEmplAccessGroupLog>'),
]

count = 0
for src, dst in t:
    if src in content:
        content = content.replace(src, dst)
        count += 1
    else:
        print(f'\ubbf8\ubc1c\uacac: {repr(src[:60])}')

with open(file, 'w', encoding='utf-8') as f:
    f.write(content)
print(f'\uc644\ub8cc: {count}\uac1c')
