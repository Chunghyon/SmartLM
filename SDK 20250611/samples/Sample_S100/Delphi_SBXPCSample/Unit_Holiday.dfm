object frmHoliday: TfrmHoliday
  Left = 258
  Top = 255
  Width = 643
  Height = 482
  Caption = 'frmHoliday'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnCreate = FormCreate
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label3: TLabel
    Left = 168
    Top = 64
    Width = 43
    Height = 19
    Caption = 'Period:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object lstHoliday: TListBox
    Left = 16
    Top = 96
    Width = 481
    Height = 337
    ItemHeight = 13
    TabOrder = 0
    OnClick = lstHolidayClick
  end
  object cmdUpdate: TButton
    Left = 512
    Top = 96
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
  object dtHoliday: TDateTimePicker
    Left = 16
    Top = 56
    Width = 89
    Height = 27
    Date = 40780.430629699080000000
    Format = 'dd/MM'
    Time = 40780.430629699080000000
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 5
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
    TabOrder = 6
  end
  object txtPeriod: TEdit
    Left = 216
    Top = 56
    Width = 81
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 7
  end
end
