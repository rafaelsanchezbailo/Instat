﻿' Instat-R
' Copyright (C) 2015
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License k
' along with this program.  If not, see <http://www.gnu.org/licenses/>.
Imports instat.Translations
Public Class dlgFitCorruptionModel
    Private bFirstLoad As Boolean = True
    Private bReset As Boolean = True
    Dim bUseSelectedColumn As Boolean = False
    Dim strSelectedColumn As String = ""
    Dim strSelectedDataFrame As String = ""
    Private bResetSubdialog As Boolean = False
    Private clsCorruptionModel, clsBinomialModel As New RFunction
    Private clsModel, clsModel1 As New ROperator

    Private Sub dlgFitCorruptionModel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        autoTranslate(Me)
        If bFirstLoad Then
            InitialiseDialog()
            bFirstLoad = False
        End If
        If bUseSelectedColumn Then
            SetDefaultColumn()
        End If
        If bReset Then
            SetDefaults()
        End If
        SetRCodeForControls(bReset)
        bReset = False

    End Sub

    Private Sub InitialiseDialog()
        'helpID
        '  ucrBase.iHelpTopicID =
        ucrInputModelPreview.IsReadOnly = True
        clsModel.SetOperation("~")
        clsModel1.SetOperation("+")
        clsModel.AddParameter(clsROperatorParameter:=clsModel1)
        clsModel1.bBrackets = False

        clsCorruptionModel.AddParameter("family", clsRFunctionParameter:=clsBinomialModel)
        clsBinomialModel.SetRCommand("binomial")
        clsBinomialModel.AddParameter("link", Chr(34) & "logit" & Chr(34))

        'Selector
        ucrSelectorFitModel.SetParameter(New RParameter("data", 0))
        ucrSelectorFitModel.SetParameterIsString()

        'Receivers
        ucrReceiverOutput.SetParameter(New RParameter("y", 1))
        ucrReceiverOutput.SetParameterIsRFunction() ' i think isRfunction here
        ucrReceiverOutput.Selector = ucrSelectorFitModel
        ucrReceiverOutput.SetIncludedDataTypes({"integer", "numeric", "logical"})

        ucrReceiverControlVariables.SetParameter(New RParameter("x", 2))
        ucrReceiverControlVariables.SetParameterIsString()
        ucrReceiverControlVariables.Selector = ucrSelectorFitModel

        ucrReceiverIndicators.SetParameter(New RParameter("z", 3))
        ucrReceiverIndicators.SetParameterIsString()
        ucrReceiverIndicators.Selector = ucrSelectorFitModel

        'ucrSave
        ucrSaveCorruptionModel.SetPrefix("Corruption_Model")
        ucrSaveCorruptionModel.SetSaveTypeAsModel()
        ucrSaveCorruptionModel.SetDataFrameSelector(ucrSelectorFitModel.ucrAvailableDataFrames)
        ucrSaveCorruptionModel.SetCheckBoxText("Save Graph")
        ucrSaveCorruptionModel.SetIsComboBox()
        ucrSaveCorruptionModel.SetAssignToIfUncheckedValue("last_graph")
    End Sub

    Private Sub SetDefaults()
        clsCorruptionModel = New RFunction

        'Reset 
        ucrSelectorFitModel.Reset()
        ucrSaveCorruptionModel.Reset()
        ucrInputModelPreview.SetName("")

        clsCorruptionModel.SetRCommand("glm")
        clsCorruptionModel.SetAssignTo("last_graph", strTempDataframe:=ucrSelectorFitModel.ucrAvailableDataFrames.cboAvailableDataFrames.Text, strTempGraph:="last_graph")

        ucrBase.clsRsyntax.SetBaseRFunction(clsCorruptionModel)
        bResetSubdialog = True
        ChangeBaseFunction()
    End Sub

    Private Sub SetCurrentColumn(strColumn As String, strDataFrame As String)
        strSelectedColumn = strColumn
        strSelectedDataFrame = strDataFrame
        bUseSelectedColumn = True
    End Sub

    Private Sub SetDefaultColumn()
        ucrSelectorFitModel.ucrAvailableDataFrames.cboAvailableDataFrames.SelectedItem = strSelectedDataFrame
        ucrReceiverControlVariables.Add(strSelectedColumn, strSelectedDataFrame)
        bUseSelectedColumn = False
    End Sub

    Private Sub SetRCodeForControls(bReset As Boolean)
        SetRCode(Me, ucrBase.clsRsyntax.clsBaseFunction, bReset)
        '        ucrReceiverOutput.SetRCode(clsModel, bReset)
        '        ucrReceiverIndicators.SetRCode(clsModel1, bReset)
        '        ucrReceiverOutput.SetRCode(clsModel1, bReset)
    End Sub

    Private Sub TestOKEnabled()
        If Not ucrReceiverOutput.IsEmpty AndAlso Not ucrReceiverIndicators.IsEmpty AndAlso Not ucrReceiverControlVariables.IsEmpty AndAlso ucrSaveCorruptionModel.IsComplete Then
            ucrBase.OKEnabled(True)
            ucrBase.clsRsyntax.RemoveParameter("formula")
            ucrBase.clsRsyntax.clsBaseFunction.AddParameter("formula", clsROperatorParameter:=clsModel)
            ucrInputModelPreview.SetName(clsModel.ToScript)
        Else
            ucrBase.OKEnabled(False)
        End If
    End Sub

    Private Sub ChangeBaseFunction()
        If ucrReceiverOutput.strCurrDataType = "numeric" OrElse ucrReceiverOutput.strCurrDataType = "integer" Then
            ucrBase.clsRsyntax.SetFunction("lm")
        Else
            ucrBase.clsRsyntax.SetFunction("glm")
        End If
    End Sub

    Private Sub SetFormula()
        If Not ucrReceiverOutput.IsEmpty AndAlso Not ucrReceiverIndicators.IsEmpty AndAlso Not ucrReceiverControlVariables.IsEmpty Then
            ucrBase.clsRsyntax.RemoveParameter("formula")
            clsModel.AddParameter(iPosition:=0, clsRFunctionParameter:=ucrReceiverOutput.GetVariables())
            clsModel.AddParameter(iPosition:=1, clsROperatorParameter:=clsModel1)
            clsModel1.AddParameter(iPosition:=0, strParameterValue:=ucrReceiverControlVariables.GetVariableNames(False))
            clsModel1.AddParameter(iPosition:=1, strParameterValue:=ucrReceiverIndicators.GetVariableNames(False))
            ' not sure about the right hand side of the formula
            ucrBase.clsRsyntax.clsBaseFunction.AddParameter("formula", clsROperatorParameter:=clsModel)
            ucrInputModelPreview.SetName(clsModel.ToScript)
        End If
    End Sub

    Private Sub ucrBase_ClickReset(sender As Object, e As EventArgs) Handles ucrBase.ClickReset
        SetDefaults()
        SetRCodeForControls(True)
        TestOKEnabled()
    End Sub

    Private Sub Controls_ControlContentsChanged(ucrChangedControl As ucrCore) Handles ucrReceiverOutput.ControlContentsChanged, ucrReceiverIndicators.ControlContentsChanged, ucrReceiverControlVariables.ControlContentsChanged, ucrSaveCorruptionModel.ControlContentsChanged
        TestOKEnabled()
    End Sub

    Private Sub cmdDisplayOptions_Click(sender As Object, e As EventArgs) Handles cmdDisplayOptions.Click
        '        sdgSimpleRegOptions.SetRFunction(ucrBase.clsRsyntax.clsBaseFunction, bResetSubdialog)
        bResetSubdialog = False
        sdgSimpleRegOptions.Show()
    End Sub

    Private Sub ucrReceiverOutput_ControlValueChanged(ucrChangedControl As ucrCore) Handles ucrReceiverOutput.ControlValueChanged, ucrReceiverControlVariables.ControlValueChanged, ucrReceiverIndicators.ControlValueChanged
        ChangeBaseFunction()
        SetFormula()
    End Sub
End Class