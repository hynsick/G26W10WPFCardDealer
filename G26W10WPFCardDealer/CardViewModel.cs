/*using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace G26W10WPFCardDealer;

public partial class CardViewModel(
    CardModel model
) : ObservableObject
{

    [ObservableProperty]
    private ObservableCollection<string>_cardResource = [..
        Enumerable.Repeat("Images/black_joker.png", CardModel.NUMBER_OF_CARDS)
    ];

    [RelayCommand]
    public void DealCards()
    {
        model.DealCards();

        for (int i = 0; i < model.Cards.Count; i++)
        {
            CardResource[i] = GetCardImageName(model.Cards[i]);
        }
    }

    private string GetCardImageName(Card card)
    {
        string[] suits = ["spades", "diamonds", "hearts", "clubs"];
        string[] values = ["ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "jack", "queen", "king"];

        if (card.Suit < 0 || card.Value < 0)
            return "Images/black_joker.png";

        string suit = suits[card.Suit];
        string value = values[card.Value];

        return value is "jack" or "queen" or "king"
            ? $"Images/{value}_of_{suit}2.png"
            : $"Images/{value}_of_{suit}.png";
    }
}*/ 
//과제 1코드
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

using PokerCard = G26W07WFCardDealer.Card;
using PokerRank = G26W07WFCardDealer.Rank;

namespace G26W10WPFCardDealer;

public partial class CardViewModel : ObservableObject
{
    private readonly CardModel model;

    public string[] HandNames { get; } =
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

    [ObservableProperty]
    private ObservableCollection<string> cardResource =
    [
        .. Enumerable.Repeat(
            "pack://application:,,,/images/black_joker.png",
            CardModel.NUMBER_OF_CARDS)
    ];

    [ObservableProperty]
    private int selectedIndex = 0;

    [ObservableProperty]
    private string resultText = "카드를 분배해주세요.";

    [ObservableProperty]
    private string expectedText = "";

    [ObservableProperty]
    private string testResultText = "";

    [ObservableProperty]
    private string testResultColor = "White";

    public CardViewModel(CardModel model)
    {
        this.model = model;
    }

    // 랜덤 카드 분배
    [RelayCommand]
    public void DealCards()
    {
        List<PokerCard> hand = model.DealCards();

        DisplayCards(hand);

        ResultText = $"포커 족보 : {model.Evaluate(hand)}";

        ExpectedText = "";
        TestResultText = "";
    }

    // 선택한 족보 테스트
    [RelayCommand]
    public void TestHand()
    {
        if (SelectedIndex < 0 ||
            SelectedIndex >= model.TestCount)
            return;

        RunTest(SelectedIndex);
    }

    // 다음 족보 테스트
    [RelayCommand]
    public void NextTest()
    {
        SelectedIndex =
            (SelectedIndex + 1) % model.TestCount;

        RunTest(SelectedIndex);
    }

    private void RunTest(int index)
    {
        List<PokerCard> hand = model.GetTestHand(index);

        DisplayCards(hand);

        string expected = HandNames[index];
        string actual = model.Evaluate(hand);

        ExpectedText = $"예상 족보 : {expected}";
        ResultText = $"실제 족보 : {actual}";

        if (expected == actual)
        {
            TestResultText = "PASS - 판별 성공";
            TestResultColor = "LightGreen";
        }
        else
        {
            TestResultText = "FAIL - 판별 실패";
            TestResultColor = "Red";
        }
    }

    private void DisplayCards(List<PokerCard> hand)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            CardResource[i] = GetCardImageName(hand[i]);
        }
    }

    private string GetCardImageName(PokerCard card)
    {
        string suit = card.Suit.ToString().ToLower();

        string value = card.Rank switch
        {
            PokerRank.Ace => "ace",
            PokerRank.Jack => "jack",
            PokerRank.Queen => "queen",
            PokerRank.King => "king",
            _ => ((int)card.Rank).ToString()
        };

        if (card.Rank is PokerRank.Jack
            or PokerRank.Queen
            or PokerRank.King)
        {
            suit += "2";
        }

        return
            $"pack://application:,,,/images/{value}_of_{suit}.png";
    }
}
