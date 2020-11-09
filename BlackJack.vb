Module Module1

    Dim SUITS As String = "CSHD"
    Dim CARDS As String = "A234567890JQKA234567890JQKA234567890JQKA234567890JQK"

    Dim playerTot As Integer
    Dim dealerTot As Integer

    ''' <summary>
    ''' Returns a randomly drawn card out of 52 possible cards, removing it from the deck for future reference
    ''' </summary>
    ''' <returns>String "Value,Suit"</returns>
    Function drawCard(ByVal player As Char) As String

        Dim VALUE As String = ""
        Dim SUIT As String = ""

        While VALUE = "" Or VALUE = "n"

            Dim randPos As Integer = Int(Rnd() * 52)

            VALUE = Mid(CARDS, randPos + 1, 1)
            SUIT = Mid(SUITS, Int((randPos) / 13) + 1, 1)

            CARDS = Mid(CARDS, 1, randPos) & "n" & Mid(CARDS, randPos + 2, CARDS.Length - (randPos + 1))
        End While

        Select Case VALUE
            Case "A"
                If player = "p" Then
                    Dim decision As Integer = 0
                    Console.WriteLine("Ace Gained, Player, would you like it to count as 1 or 11? Enter 1, 11")

                    While decision <> 1 And decision <> 11
                        Try
                            Console.Write("> ")
                            decision = Convert.ToInt32(Console.ReadLine())
                        Catch
                            ' do nothing
                        End Try
                    End While

                    playerTot += 11

                    If decision = 1 Then
                        playerTot -= 10
                    End If
                Else
                    Console.WriteLine("Ace Gained, Dealer, would you like it to count as 1 or 11? Enter 1, 11")
                    Console.Write("> ")

                    Threading.Thread.Sleep(500)

                    If dealerTot <= 10 Then
                        dealerTot += 11
                        Console.WriteLine("11")
                    Else
                        dealerTot += 1
                        Console.WriteLine("1")
                    End If
                End If
            Case "J", "K", "Q"
                If player = "p" Then
                    playerTot += 10
                Else
                    dealerTot += 10
                End If
            Case "0"
                If player = "p" Then
                    playerTot += 10
                Else
                    dealerTot += 10
                End If
            Case Else
                If player = "p" Then
                    playerTot += Int(VALUE)
                Else
                    dealerTot += Int(VALUE)
                End If
        End Select
        If player = "p" Then
            Console.Write("Player was given card ")
        Else
            Console.Write("Dealer was given card ")
        End If
        displayCard(VALUE & "," & SUIT)

        Return (VALUE & "," & SUIT)
    End Function

    ''' <summary>
    ''' Returns 2 random cards returned from drawCard Function
    ''' </summary>
    ''' <returns>ChosenCards</returns>
    Function getHand(ByVal user As Char) As String()
        Dim ChosenCards(2) As String

        For i = 0 To 1
            ChosenCards(i) = drawCard(user)
        Next

        Return ChosenCards
    End Function

    Function takeTurn(ByVal user As Char, ByVal h() As String) As String

        Dim newCard As String = ""
        Dim decision As String = ""

        If user = "p" Then
            Console.WriteLine("Stick (s) or Twist? (t) - Your current card values add up to: " & playerTot)

            While decision <> "t" And decision <> "s"
                Console.Write("> ")

                Try
                    decision = Console.ReadLine().ToLower()
                Catch
                    ' do nothing
                End Try
            End While

            If decision = "t" Then
                Return drawCard(user)
            End If

            If decision = "s" Then
                Return "n"
            End If

        ElseIf user = "d" Then
            If dealerTot > 17 Then
                Return "n"
            Else
                Return drawCard(user)
            End If
        End If

    End Function

    Function displayCard(ByVal card As String) As String

        Dim card_val As String = Mid(card, 1, 1)
        Dim card_suit As String = Mid(card, 3, 1)

        Dim card_disp As String = ""

        If card <> "" Then
            Select Case card_val
                Case "A"
                    card_disp += "Ace of"
                Case "J"
                    card_disp += "Jack of"
                Case "Q"
                    card_disp += "Queen of"
                Case "K"
                    card_disp += "King of"
                Case "0"
                    card_disp += "10 of"
                Case Else
                    card_disp += card_val & " of"
            End Select

            Select Case card_suit
                Case "C"
                    card_disp += " Clubs"
                    Console.ForegroundColor = ConsoleColor.Black
                Case "S"
                    card_disp += " Spades"
                    Console.ForegroundColor = ConsoleColor.Black
                Case "D"
                    card_disp += " Diamonds"
                    Console.ForegroundColor = ConsoleColor.Red
                Case "H"
                    card_disp += " Hearts"
                    Console.ForegroundColor = ConsoleColor.Red
            End Select

            Console.BackgroundColor = ConsoleColor.White
            Console.WriteLine(card_disp)
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White

        End If

    End Function

    Sub Main()

        Dim player_hand() As String
        Dim dealer_hand() As String

        Randomize()

        player_hand = getHand("p")
        dealer_hand = getHand("d")

        Console.WriteLine("Your Hand: ")

        For i = 0 To player_hand.Length - 1
            displayCard(player_hand(i))
        Next

        While player_hand(player_hand.Length - 1) <> "n"

            ReDim Preserve player_hand(player_hand.Length)

            player_hand(player_hand.Length - 1) = takeTurn("p", player_hand)

            If player_hand(player_hand.Length - 1) <> "n" Then
                Console.WriteLine()
                Console.WriteLine("Your Hand: ")

                For i = 0 To player_hand.Length - 1
                    displayCard(player_hand(i))
                Next

                If playerTot > 21 Then
                    Console.WriteLine("You drew a card and your score exceeded 21, you have gone bust!")
                    Exit While

                End If

            End If

        End While

        Console.WriteLine()
        Console.WriteLine("Dealer Total: " & dealerTot)
        Console.WriteLine("Dealer Hand:")

        For i = 0 To dealer_hand.Length - 1
            displayCard(dealer_hand(i))
        Next

        While dealerTot < 17

            Console.WriteLine("Dealer Total: " & dealerTot)
            Console.WriteLine("Dealer Hand:")

            ReDim Preserve dealer_hand(dealer_hand.Length)
            dealer_hand(dealer_hand.Length - 1) = takeTurn("d", dealer_hand)

            For i = 0 To dealer_hand.Length - 1
                displayCard(dealer_hand(i))
            Next

        End While

        Console.BackgroundColor = ConsoleColor.Black
        Console.WriteLine()
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("Your Total: " & playerTot)
        Console.WriteLine("Dealer Total: " & dealerTot)
        Console.WriteLine()

        If playerTot > 21 And dealerTot > 21 Then
            Console.WriteLine("Draw! you both went bust!")
        ElseIf playerTot > dealerTot And playerTot < 22 Then
            Console.WriteLine("You win!")
        ElseIf dealerTot > playerTot And dealerTot < 22 Then
            Console.WriteLine("Dealer wins!")
        ElseIf playerTot < dealerTot And dealerTot > 21 Then
            Console.WriteLine("You win, Dealer went bust!")
        ElseIf dealerTot < playerTot And playerTot > 21 Then
            Console.WriteLine("Dealer wins, You went bust!")
        End If

        Console.WriteLine("Any key to exit")
        Console.ReadKey()

    End Sub

End Module
