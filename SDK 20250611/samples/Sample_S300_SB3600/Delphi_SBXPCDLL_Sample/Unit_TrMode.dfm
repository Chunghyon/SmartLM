object frmTrMode: TfrmTrMode
  Left = 390
  Top = 420
  Width = 651
  Height = 484
  Caption = 'frmTrMode'
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
    Width = 66
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
    Width = 61
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
    Width = 91
    Height = 19
    Caption = 'In / Out Status:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object lstTrMode: TListBox
    Left = 16
    Top = 152
    Width = 481
    Height = 281
    ImeName = 'Korean Input System (IME 2000)'
    ItemHeight = 13
    TabOrder = 0
    OnClick = lstTrModeClick
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
    Left = 512
    Top = 296
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
    Left = 512
    Top = 344
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
    Left = 512
    Top = 392
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
  object cmbTrStatus: TComboBox
    Left = 272
    Top = 96
    Width = 201
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ItemHeight = 19
    ParentFont = False
    TabOrder = 5
    Text = 'cmbTrStatus'
    Items.Strings = (
      'Duty On'
      'Duty Off'
      'OverTime On'
      'OverTime Off'
      'Return'
      'Go Out')
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
    Font.Style = []
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
    Font.Style = []
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
