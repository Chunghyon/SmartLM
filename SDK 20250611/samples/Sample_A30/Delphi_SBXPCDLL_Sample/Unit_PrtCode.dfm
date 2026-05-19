object frmPrtCode: TfrmPrtCode
  Left = 334
  Top = 205
  Width = 529
  Height = 258
  Caption = 'Product Code'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 40
    Top = 64
    Width = 106
    Height = 19
    Caption = 'Serial Number :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object Label3: TLabel
    Left = 40
    Top = 104
    Width = 99
    Height = 19
    Caption = 'Product Code :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object lblMessage: TStaticText
    Left = 8
    Top = 16
    Width = 505
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
  object txtSerialNo: TEdit
    Left = 168
    Top = 64
    Width = 321
    Height = 27
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 1
  end
  object txtProductCode: TEdit
    Left = 168
    Top = 104
    Width = 321
    Height = 27
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 2
  end
  object cmdGetSerialNumber: TButton
    Left = 56
    Top = 160
    Width = 137
    Height = 41
    Caption = 'Get   SerialNumber'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 3
    WordWrap = True
    OnClick = cmdGetSerialNumberClick
  end
  object cmdGetProductCode: TButton
    Left = 216
    Top = 160
    Width = 129
    Height = 41
    Caption = 'Get   ProductCode'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 4
    WordWrap = True
    OnClick = cmdGetProductCodeClick
  end
  object cmdExit: TButton
    Left = 368
    Top = 160
    Width = 89
    Height = 41
    Caption = 'Exit'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 5
    WordWrap = True
    OnClick = cmdExitClick
  end
end
