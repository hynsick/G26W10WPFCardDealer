using G26W07WFCardDealer;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using PokerCard = G26W07WFCardDealer.Card;
using PokerRank = G26W07WFCardDealer.Rank;

namespace G26W10WPFCardDealer;

public partial class MainWindow : Window
{
    // CheckCardTest.cs의 테스트 데이터 순서와 동일하게 구성
    private readonly string[] handNames =
    {
        "로열 스트레이트 플러시",
        "백스트레이트 플러시",
        "스트레이트 플러시",
        "포카드",
        "풀하우스",
        "플러시",
        "마운틴",
        "백스트레이트",
        "스트레이트",
        "트리플",
        "투 페어",
        "원 페어",
        "하이 카드"
    };

    public MainWindow()
    {
        InitializeComponent();

        // ComboBox에 족보 목록 등록
        HandComboBox.ItemsSource = handNames;

        // 첫 번째 족보 선택
        HandComboBox.SelectedIndex = 0;
    }

    // 랜덤 카드 분배
    private void OnDeal(object sender, RoutedEventArgs e)
    {
        Deck deck = new Deck();

        List<PokerCard> hand = deck.Draw(5);

        DisplayCards(hand);

        string result = CheckCard.Evaluate(hand);

        ResultText.Text = $"포커 족보 : {result}";

        ExpectedText.Text = "";
        TestResultText.Text = "";
    }

    // 선택한 족보 테스트
    private void OnTest(object sender, RoutedEventArgs e)
    {
        int index = HandComboBox.SelectedIndex;

        if (index < 0)
            return;

        RunTest(index);
    }

    // 다음 족보 테스트
    private void OnNextTest(object sender, RoutedEventArgs e)
    {
        int index = HandComboBox.SelectedIndex;

        index = (index + 1) % CheckCardTest.Count;

        HandComboBox.SelectedIndex = index;

        RunTest(index);
    }

    // 족보 테스트 공통 메서드
    private void RunTest(int index)
    {
        // 기존 테스트 데이터 가져오기
        List<PokerCard> hand = CheckCardTest.GetHand(index);

        // 카드 이미지 출력
        DisplayCards(hand);

        // 예상 족보
        string expected = handNames[index];

        // 기존 알고리즘을 이용한 실제 판별
        string actual = CheckCard.Evaluate(hand);

        ExpectedText.Text = $"예상 족보 : {expected}";

        ResultText.Text = $"실제 족보 : {actual}";

        // 예상 결과와 실제 결과 비교
        if (expected == actual)
        {
            TestResultText.Text = "PASS - 판별 성공";
            TestResultText.Foreground = Brushes.LightGreen;
        }
        else
        {
            TestResultText.Text = "FAIL - 판별 실패";
            TestResultText.Foreground = Brushes.Red;
        }
    }

    // 카드 이미지 출력 공통 메서드
    private void DisplayCards(List<PokerCard> hand)
    {
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

            // J, Q, K 이미지 파일명 처리
            if (card.Rank is PokerRank.Jack
                or PokerRank.Queen
                or PokerRank.King)
            {
                suit += "2";
            }

            string imagePath =
                $"pack://application:,,,/images/{value}_of_{suit}.png";

            if (FindName($"Card{i + 1}") is Image imageCard)
            {
                imageCard.Source = new BitmapImage(
                    new Uri(imagePath, UriKind.Absolute));
            }
        }
    }
}