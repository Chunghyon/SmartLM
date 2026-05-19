object frmAccessTz: TfrmAccessTz
  Left = 306
  Top = 296
  Caption = 'frmAccessTz'
  ClientHeight = 601
  ClientWidth = 634
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 16
    Top = 64
    Width = 65
    Height = 19
    Caption = 'Start Time:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label2: TLabel
    Left = 16
    Top = 104
    Width = 60
    Height = 19
    Caption = 'End Time:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label3: TLabel
    Left = 272
    Top = 64
    Width = 79
    Height = 19
    Caption = 'Verify Mode:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object lstAccessTz: TListBox
    Left = 16
    Top = 152
    Width = 481
    Height = 425
    ImeName = 'Korean Input System (IME 2000)'
    ItemHeight = 13
    TabOrder = 0
    OnClick = lstAccessTzClick
  end
  object cmdUpdate: TButton
    Left = 512
    Top = 152
    Width = 113
    Height = 41
    Caption = 'Update'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 1
    OnClick = cmdUpdateClick
  end
  object cmdRead: TButton
    Left = 504
    Top = 440
    Width = 113
    Height = 41
    Caption = 'Read'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 2
    OnClick = cmdReadClick
  end
  object cmdWrite: TButton
    Left = 504
    Top = 488
    Width = 113
    Height = 41
    Caption = 'Write'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 3
    OnClick = cmdWriteClick
  end
  object cmdExit: TButton
    Left = 504
    Top = 536
    Width = 113
    Height = 41
    Caption = 'Exit'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 4
    OnClick = cmdExitClick
  end
  object cmbVerifyMode: TComboBox
    Left = 272
    Top = 96
    Width = 201
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 5
    Text = 'cmbVerifyMode'
    Items.Strings = (
      'FACE|FP|CD|PWD'
      'CD&FP'
      'FP&PWD'
      'CD&PWD'
      'FP&CD&PWD'
      'System Default VM'
      'FP'
      'CD'
      '8-INVALID-DON'#39'T SELECT ME'
      'ID&PWD'
      '10-INVALID-DON'#39'T SELECT ME'
      'FACE'
      'CD&FACE'
      'FACE&PWD'
      'FACE&CD&PWD'
      'FACE&FP')
  end
  object dtStart: TDateTimePicker
    Left = 112
    Top = 64
    Width = 97
    Height = 27
    Date = 40780.430629699080000000
    Format = 'HH:mm'
    Time = 40780.430629699080000000
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ImeName = 'Korean Input System (IME 2000)'
    Kind = dtkTime
    ParentFont = False
    TabOrder = 6
  end
  object dtEnd: TDateTimePicker
    Left = 112
    Top = 104
    Width = 97
    Height = 27
    Date = 40780.430629699080000000
    Format = 'HH:mm'
    Time = 40780.430629699080000000
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ImeName = 'Korean Input System (IME 2000)'
    Kind = dtkTime
    ParentFont = False
    TabOrder = 7
  end
  object lblMessage: TStaticText
    Left = 8
    Top = 16
    Width = 617
    Height = 26
    Align = alCustom
    Alignment = taCenter
    AutoSize = False
    BorderStyle = sbsSunken
    Caption = 'Message'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 8
  end
end
