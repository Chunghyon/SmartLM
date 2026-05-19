object frmLockCtrl: TfrmLockCtrl
  Left = 818
  Top = 277
  Width = 430
  Height = 213
  Caption = 'Lock Control'
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
  object lblMessage: TStaticText
    Left = 8
    Top = 10
    Width = 401
    Height = 31
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
  object cmdGetDoorStatus: TButton
    Left = 8
    Top = 56
    Width = 129
    Height = 30
    Caption = 'Get DoorStatus'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 1
    OnClick = cmdGetDoorStatusClick
  end
  object cmdDoorOpen: TButton
    Left = 144
    Top = 56
    Width = 129
    Height = 30
    Caption = 'Door Open'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 2
    OnClick = cmdDoorOpenClick
  end
  object cmdUncondOpen: TButton
    Left = 280
    Top = 56
    Width = 129
    Height = 30
    Caption = 'Uncond Open'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 3
    OnClick = cmdUncondOpenClick
  end
  object cmdAutoRecover: TButton
    Left = 144
    Top = 96
    Width = 129
    Height = 30
    Caption = 'Auto Recover'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 4
    OnClick = cmdAutoRecoverClick
  end
  object cmdUncondClose: TButton
    Left = 280
    Top = 96
    Width = 129
    Height = 30
    Caption = 'Uncond Close'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 5
    OnClick = cmdUncondCloseClick
  end
  object cmdRestart: TButton
    Left = 144
    Top = 136
    Width = 129
    Height = 30
    Caption = 'Reboot'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 6
    OnClick = cmdRestartClick
  end
  object cmdWarnCancel: TButton
    Left = 280
    Top = 136
    Width = 129
    Height = 30
    Caption = 'Warn Cancel'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 7
    OnClick = cmdWarnCancelClick
  end
end
