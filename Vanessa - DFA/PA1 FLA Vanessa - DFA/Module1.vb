Imports System.IO
Imports System.Windows.Forms

Module Module1
    <STAThread()> 'to activate the openfiledialog function in vb console
    Sub Main()
        Dim s As String
        Dim alph() As String
        Dim states() As String
        Dim final() As String

        Console.Write("Please enter the alphabet (seperated by SPACE / ' '): ")
        s = Console.ReadLine()
        alph = s.Split(" ")

        Console.Write("Please enter the states (seperated by SPACE / ' '): ")
        s = Console.ReadLine()
        states = s.Split(" ")

        Dim tTable(states.Length - 1, alph.Length - 1) As String

        Console.Write("Final state(s) (seperated by SPACE / ' '): ")
        s = Console.ReadLine()
        final = s.Split(" ")

        Dim correctF As Boolean = checkFinalState(final, states)
        While Not checkFinalState(final, states)
            Console.Write("The final state(s) does not exist. Enter again: ")
            s = Console.ReadLine()
            final = s.Split(" ")
            correctF = checkFinalState(final, states)
        End While

        Console.WriteLine()
        Console.WriteLine("Please select your file: ")

        Dim OpenFileDlg As New OpenFileDialog()
        OpenFileDlg.FileName = "" ' Default file name
        OpenFileDlg.DefaultExt = ".txt" ' Default file extension
        OpenFileDlg.Filter = "Text Files (*.txt)|*.txt"
        OpenFileDlg.Multiselect = False
        OpenFileDlg.RestoreDirectory = True

        If OpenFileDlg.ShowDialog() Then
            Dim reader As StreamReader
            Dim path As String = OpenFileDlg.FileName

            If path = "" Then
                Console.WriteLine("File not found")
            Else
                reader = New StreamReader(path)

                Dim str As String
                str = reader.ReadLine

                Dim row As Integer = 0
                Dim col As Integer

                While (Not str Is Nothing) 'loop to handle each row of the matrix
                    Dim inp As String = ""
                    col = 0

                    For i = 0 To str.Length - 1 'loop to insert each of the column in the matrix
                        If str(i) <> " " Then
                            inp += str(i)
                            If i = str.Length - 1 Then
                                tTable(row, col) = inp
                            End If
                        Else
                            tTable(row, col) = inp
                            inp = ""
                            col += 1
                        End If
                    Next

                    str = reader.ReadLine
                    row += 1
                End While

                reader.Close()

                Console.WriteLine()

                Dim ans As Char = "y"
                While ans = "y" Or ans = "Y"
                    Console.Write("Please enter an input string: ")
                    Dim input As String = Console.ReadLine

                    Dim cur As Integer = 0
                    Dim curstate As String = states(0) 'contains the states in the transition table. the order must be the same with the transition table

                    For i = 0 To input.Length - 1 'loop the input string to be checked
                        Dim j As Integer = 0
                        Dim flag1 As Boolean = False
                        While j < alph.Length And Not flag1 'loop the alphabet string whose index is correspondent to the column index of the matrix
                            If input(i) = alph(j) Then
                                curstate = tTable(cur, j)

                                Dim k As Integer = 0
                                Dim flag2 As Boolean = False
                                While k < states.Length And Not flag2
                                    If curstate = states(k) Then
                                        cur = k

                                        flag2 = True 'will turn true and exit the loop if it has found the state that is the same with the current state
                                    End If

                                    k += 1
                                End While

                                flag1 = True 'will turn true and exit the loop if the alphabet is the same with the current input char
                            End If

                            j += 1
                        End While
                    Next

                    Dim accepted As Boolean = False
                    Dim a As Integer = 0

                    While a < final.Length And Not accepted
                        If curstate = final(a) Then
                            accepted = True 'if the current state is similar to one of the final state, then the string is accepted
                        End If

                        a += 1
                    End While

                    If accepted Then
                        Console.WriteLine(input + " is accepted")
                    Else
                        Console.WriteLine(input + " is not accepted")
                    End If

                    Console.WriteLine()

                    Console.Write("Do you want to try again[Y/N]? ")
                    ans = Console.ReadLine()

                    Console.WriteLine()
                End While
            End If
        Else
            Console.WriteLine("File not found")
        End If

        Console.ReadLine()
    End Sub

    Function checkFinalState(final() As String, states() As String)
        Dim flag(final.Length - 1) As Boolean
        For i = 0 To flag.Length - 1
            flag(i) = False
        Next

        For i = 0 To final.Length - 1
            For j = 0 To states.Length - 1
                If final(i) = states(j) Then
                    flag(i) = True
                End If
            Next
        Next

        Dim b As Integer = 0
        Dim correctF As Boolean = True

        While b < flag.Length And correctF = True
            If Not flag(b) Then
                correctF = False
            End If

            b += 1
        End While

        Return correctF
    End Function
End Module