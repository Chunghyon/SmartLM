object frmLog: TfrmLog
  Left = 183
  Top = 319
  Caption = 'Log Management'
  ClientHeight = 456
  ClientWidth = 907
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
  object imgLogPhoto: TImage
    Left = 736
    Top = 108
    Width = 160
    Height = 120
    AutoSize = True
    Stretch = True
  end
  object lblMessage: TStaticText
    Left = 8
    Top = 24
    Width = 897
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
    OnClick = gridSLogDataClick
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
  object chkShowGLogPhoto: TCheckBox
    Left = 736
    Top = 72
    Width = 145
    Height = 17
    Caption = 'Show GLog Photo'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 11
  end
  object GroupBox3: TGroupBox
    Left = 736
    Top = 248
    Width = 169
    Height = 161
    Caption = 'GLog Search Range'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 12
    object Label2: TLabel
      Left = 16
      Top = 56
      Width = 32
      Height = 19
      Caption = 'Start:'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object Label3: TLabel
      Left = 16
      Top = 96
      Width = 27
      Height = 19
      Caption = 'End:'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object dtStart: TDateTimePicker
      Left = 64
      Top = 56
      Width = 97
      Height = 27
      Date = 40780.788788518520000000
      Time = 40780.788788518520000000
      ImeName = 'Korean Input System (IME 2000)'
      TabOrder = 0
    end
    object dtEnd: TDateTimePicker
      Left = 64
      Top = 88
      Width = 97
      Height = 27
      Date = 40780.788788518520000000
      Time = 40780.788788518520000000
      ImeName = 'Korean Input System (IME 2000)'
      TabOrder = 1
    end
    object chkUseSearchRange: TCheckBox
      Left = 16
      Top = 24
      Width = 137
      Height = 25
      Caption = 'Use Search Range'
      TabOrder = 2
    end
    object cmdGLogSearchRange: TButton
      Left = 32
      Top = 128
      Width = 105
      Height = 25
      Caption = 'Set Range'
      TabOrder = 3
      OnClick = cmdGLogSearchRangeClick
    end
  end
end
