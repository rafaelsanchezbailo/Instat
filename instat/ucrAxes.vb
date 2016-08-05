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

Public Class ucrAxes
    Public bIsX As Boolean
    Public clsRsyntax As New RSyntax
    Public clsRggplotFunction As New RFunction
    Public clsTitleFunction As New RFunction
    Public bFirstLoad As Boolean = True

    Private Sub ucrAxes_Load(sender As Object, e As EventArgs) Handles Me.Load
        If bFirstLoad Then
            SetDefaults()
            InitialiseControl()
            bFirstLoad = False
        End If
    End Sub

    Private Sub SetDefaults()
        rdoTitleAuto.Checked = True
        rdoScalesAuto.Checked = True
        rdoTickMarkersAuto.Checked = True
        TitleDefaults()
        ScalesDefaults()
        TickMarkersDefaults()
    End Sub

    Private Sub InitialiseControl()
        ucrTickMarkers.cboInput.Items.Add({"Interval", "Specific Values"})
        ucrTickMarkers.SetName("Interval")
        TitleDefaults()
        ScalesDefaults()
        TickMarkersDefaults()
    End Sub

    Private Sub TitleDefaults()
        If rdoTitleAuto.Checked Then
            chkDisplayTitle.Visible = False
            chkOverwriteTitle.Visible = False
            ucrOverwriteTitle.Visible = False
        ElseIf rdoTitleCustom.Checked Then
            chkDisplayTitle.Visible = True
            chkDisplayTitle.Checked = True
            chkOverwriteTitle.Visible = True
            chkOverwriteTitle.Checked = False
        End If
    End Sub

    Private Sub ScalesDefaults()
        If rdoScalesAuto.Checked Then
            lblLowerLimit.Visible = False
            nudLowerLimit.Visible = False
            lblUpperLimit.Visible = False
            nudUpperLimit.Visible = False
            lblScalesNoDecimalPlaces.Visible = False
            nudScalesNoOfDecimalPlaces.Visible = False
        ElseIf rdoScalesCustom.Checked Then
            lblLowerLimit.Visible = True
            nudLowerLimit.Visible = True
            lblUpperLimit.Visible = True
            nudUpperLimit.Visible = True
            lblScalesNoDecimalPlaces.Visible = True
            nudScalesNoOfDecimalPlaces.Visible = True
        End If
    End Sub

    Private Sub TickMarkersDefaults()
        If rdoTickMarkersAuto.Checked Then
            ucrTickMarkers.Visible = False
            lblFrom.Visible = False
            nudFrom.Visible = False
            lblTo.Visible = False
            nudTo.Visible = False
            lblInStepsOf.Visible = False
            nudInStepsOf.Visible = False
            lblTickMarkersNoOfDecimalPlaces.Visible = False
            nudTickMarkersNoOfDecimalPlaces.Visible = False
            ucrSpecificValues.Visible = False
        ElseIf rdoTickMarkersCustom.Checked Then
            ucrTickMarkers.Visible = False
            lblFrom.Visible = True
            nudFrom.Visible = True
            lblTo.Visible = True
            nudTo.Visible = True
            lblInStepsOf.Visible = True
            nudInStepsOf.Visible = True
            lblTickMarkersNoOfDecimalPlaces.Visible = True
            nudTickMarkersNoOfDecimalPlaces.Visible = True
            ucrSpecificValues.Visible = False
        End If
    End Sub

    Public Sub SetXorY(bIsX)
        If bIsX = True Then
            clsTitleFunction.SetRCommand("xlab")
            ' put scale_x_continuous function here
        ElseIf bIsX = False Then
            clsTitleFunction.SetRCommand("ylab")
            ' put scale_y_continuous function here
        End If
    End Sub

    Public Sub SetRsyntaxAxis(clsRsyntaxAxis As RSyntax)
        clsRsyntax = clsRsyntaxAxis
    End Sub

    Private Sub TitleFunction()
        If rdoTitleCustom.Checked AndAlso chkDisplayTitle.Checked Then
            If chkOverwriteTitle.Checked AndAlso Not ucrOverwriteTitle.IsEmpty Then
                clsTitleFunction.AddParameter("label", Chr(34) & ucrOverwriteTitle.GetText & Chr(34))
                clsRsyntax.AddOperatorParameter("axistitle", clsRFunc:=clsTitleFunction)
            Else
                clsRsyntax.RemoveOperatorParameter("axistitle")
            End If
        Else
            clsTitleFunction.AddParameter("label", Chr(34) & "" & Chr(34))
            clsRsyntax.AddOperatorParameter("axistitle", clsRFunc:=clsTitleFunction)
        End If
    End Sub

    Private Sub ucrOverwriteTitle_NameChanged() Handles ucrOverwriteTitle.NameChanged
        If rdoTitleCustom.Checked Then
            TitleFunction()
        End If
    End Sub

    Private Sub chkOverwriteTitle_CheckedChanged(sender As Object, e As EventArgs) Handles chkOverwriteTitle.CheckedChanged
        If rdoTitleCustom.Checked AndAlso chkOverwriteTitle.Checked Then
            ucrOverwriteTitle.Visible = True
        Else
            ucrOverwriteTitle.Visible = False
        End If
        TitleFunction()
    End Sub

    Private Sub chkDisplayTitle_CheckedChanged(sender As Object, e As EventArgs) Handles chkDisplayTitle.CheckedChanged
        If rdoTitleCustom.Checked AndAlso chkDisplayTitle.Checked Then
            chkOverwriteTitle.Visible = True
            If chkOverwriteTitle.Checked Then
                ucrOverwriteTitle.Visible = True
            Else
                ucrOverwriteTitle.Visible = False
            End If
        Else
            chkOverwriteTitle.Visible = False
            ucrOverwriteTitle.Visible = False
        End If
        TitleFunction()
    End Sub

    Private Sub rdoTitleCustom_CheckedChanged(sender As Object, e As EventArgs) Handles rdoTitleCustom.CheckedChanged
        TitleDefaults()
    End Sub

    Private Sub rdoTitleAuto_CheckedChanged(sender As Object, e As EventArgs) Handles rdoTitleAuto.CheckedChanged
        TitleDefaults()
    End Sub

    Private Sub rdoScalesCustom_CheckedChanged(sender As Object, e As EventArgs) Handles rdoScalesCustom.CheckedChanged
        ScalesDefaults()
    End Sub

    Private Sub rdoScalesAuto_CheckedChanged(sender As Object, e As EventArgs) Handles rdoScalesAuto.CheckedChanged
        ScalesDefaults()
    End Sub

    Private Sub rdoTickMarkersAuto_CheckedChanged(sender As Object, e As EventArgs) Handles rdoTickMarkersAuto.CheckedChanged
        TickMarkersDefaults()
    End Sub

    Private Sub rdoTickMarkersCustom_CheckedChanged(sender As Object, e As EventArgs) Handles rdoTickMarkersCustom.CheckedChanged
        TickMarkersDefaults()
    End Sub
End Class