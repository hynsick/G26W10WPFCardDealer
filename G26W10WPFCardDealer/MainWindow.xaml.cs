// Homework #1의 Business Logic
using G26W07WFCardDealer;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
// 기존 WPF Card 클래스와 이름 충돌 방지
using PokerCard = G26W07WFCardDealer.Card;
using PokerRank = G26W07WFCardDealer.Rank;

namespace G26W10WPFCardDealer;

public partial class MainWindow : System.Windows.Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnDeal(object sender, RoutedEventArgs e)
    {
        // 1. 기존 Deck 클래스를 이용하여 카드 5장 생성
        Deck deck = new Deck();
        List<PokerCard> hand = deck.Draw(5);

        // 2. 카드 이미지 출력
        for (int i = 0; i < hand.Count; i++)
        {
            PokerCard card = hand[i];

            string suit = card.Suit.ToString().ToLower();

            string value = card.Rank switch
            {
                PokerRank.Ace => "ace",
                PokerRank.Jack => "jack",
                PokerRank.Queen => "queen",
                PokerRank.King => "king",
                _ => ((int)card.Rank).ToString()
            };

            // J, Q, K 카드 이미지 파일명 처리
            if (card.Rank is PokerRank.Jack
                or PokerRank.Queen
                or PokerRank.King)
            {
                suit += "2";
            }

            string imagePath = $"/images/{value}_of_{suit}.png";

            if (FindName($"Card{i + 1}") is Image imageCard)
            {
                imageCard.Source = new BitmapImage(
                   new Uri(imagePath, UriKind.Relative));
            }
        }

        // 3. 기존 포커 족보 판별 알고리즘 재사용
        string result = CheckCard.Evaluate(hand);

        // 4. WPF 화면에 결과 출력
        ResultText.Text = $"포커 족보 : {result}";
    }
}