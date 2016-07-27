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
Public Class dlgColumnStructure
    Public bFirstLoad As Boolean = True
    Private Sub ucrSelectorColumnStructures_Load(sender As Object, e As EventArgs) Handles Me.Load
        autoTranslate(Me)
        If bFirstLoad Then
            SetDefaults()
            InitialiseDialog()
            bFirstLoad = False
        Else
            ReopenDialog()
        End If
        'Checks if Ok can be enabled.
        TestOKEnabled()
    End Sub

    Private Sub InitialiseDialog()
        ucrBase.clsRsyntax.iCallType = 2
        ucrReceiverTreatment.Selector = ucrSelectorColumnStructure
        ucrReceiverForLayout.Selector = ucrSelectorColumnStructure
        ucrReceiverMeasurement.Selector = ucrSelectorColumnStructure
        ucrReceiverForLayout.SetMeAsReceiver()
    End Sub

    Private Sub SetDefaults()

    End Sub

    Private Sub ReopenDialog()

    End Sub

    Private Sub TestOKEnabled()

    End Sub

    Private Sub ucrBase_ClickReset(sender As Object, e As EventArgs)
        SetDefaults()
        TestOKEnabled()
    End Sub
End Class