object frmHoliday: TfrmHoliday
  Left = 554
  Top = 240
  Width = 659
  Height = 591
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
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 16
    Top = 64
    Width = 65
    Height = 19
    Caption = 'Start Date:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label2: TLabel
    Left = 248
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
  object lstData: TListBox
    Left = 16
    Top = 112
    Width = 481
    Height = 425
    ImeName = 'Korean Input System (IME 2000)'
    ItemHeight = 13
    TabOrder = 0
    OnClick = lstDataClick
  end
  object cmdUpdate: TButton
    Left = 512
    Top = 112
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
    Top = 400
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
    Top = 448
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
    Top = 496
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
    Left = 104
    Top = 64
    Width = 113
    Height = 27
    Date = 40780.430629699080000000
    Format = 'dd/MM'
    Time = 40780.430629699080000000
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ImeName = 'Korean Input System (IME 2000)'
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
    Left = 304
    Top = 64
    Width = 105
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 7
    Text = '0'
  end
end
