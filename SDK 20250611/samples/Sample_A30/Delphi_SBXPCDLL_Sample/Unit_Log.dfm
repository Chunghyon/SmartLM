object frmLog: TfrmLog
  Left = 183
  Top = 319
  Width = 745
  Height = 495
  Caption = 'Log Management'
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
    Left = 24
    Top = 72
    Width = 64
    Height = 19
    Caption = 'Log Data :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object LabelTotal: TLabel
    Left = 112
    Top = 72
    Width = 37
    Height = 19
    Caption = 'Total :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object lblMessage: TStaticText
    Left = 8
    Top = 24
    Width = 705
    Height = 33
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
    TabOrder = 0
  end
  object gridSLogData: TStringGrid
    Left = 8
    Top = 96
    Width = 713
    Height = 289
    ColCount = 9
    DefaultRowHeight = 17
    RowCount = 2
    TabOrder = 1
  end
  object chkAndDelete: TCheckBox
    Left = 496
    Top = 72
    Width = 97
    Height = 17
    Caption = 'and Delete'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
  end
  object chkReadMark: TCheckBox
    Left = 616
    Top = 72
    Width = 97
    Height = 17
    Caption = 'ReadMark'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 3
  end
  object cmdSLogData: TButton
    Left = 8
    Top = 400
    Width = 97
    Height = 49
    Caption = 'Read SLogData'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 4
    WordWrap = True
    OnClick = cmdSLogDataClick
  end
  object cmdAllSLogData: TButton
    Left = 112
    Top = 400
    Width = 97
    Height = 49
    Caption = 'Read All SLogData'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 5
    WordWrap = True
    OnClick = cmdAllSLogDataClick
  end
  object cmdEmptySLog: TButton
    Left = 216
    Top = 400
    Width = 97
    Height = 49
    Caption = 'Empty SLogData'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 6
    WordWrap = True
    OnClick = cmdEmptySLogClick
  end
  object cmdGlogData: TButton
    Left = 320
    Top = 400
    Width = 97
    Height = 49
    Caption = 'Read GLogData'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 7
    WordWrap = True
    OnClick = cmdGlogDataClick
  end
  object cmdAllGLogData: TButton
    Left = 424
    Top = 400
    Width = 97
    Height = 49
    Caption = 'Read All GLogData'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 8
    WordWrap = True
    OnClick = cmdAllGLogDataClick
  end
  object cmdEmptyGLog: TButton
    Left = 528
    Top = 400
    Width = 97
    Height = 49
    Caption = 'Empty GLogData'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 9
    WordWrap = True
    OnClick = cmdEmptyGLogClick
  end
  object cmdExit: TButton
    Left = 632
    Top = 400
    Width = 89
    Height = 49
    Caption = 'Exit'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 10
    WordWrap = True
    OnClick = cmdExitClick
  end
end
