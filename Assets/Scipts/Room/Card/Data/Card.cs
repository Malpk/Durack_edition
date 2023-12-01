using System.Collections.Generic;

public class Card //это решение взято из старого клиента, потому что либо это, либо заставить серверщика встать с дивана, что сложно
{
    public string suit;
    public string nominal;

    private Dictionary<string, ENominal> _niminal = new Dictionary<string, ENominal>
        {
            { "2 ", ENominal.TWO    },
            { "3 ", ENominal.THREE  },
            { "4 ", ENominal.FOUR   },
            { "5 ", ENominal.FIVE   },
            { "6 ", ENominal.SIX    },
            { "7 ", ENominal.SEVEN  },
            { "8 ", ENominal.EIGHT  },
            { "9 ", ENominal.NINE   },
            { "10", ENominal.TEN    },
            { "В ", ENominal.JACK   },
            { "Д ", ENominal.QUEEN  },
            { "К ", ENominal.KING   },
            { "Т ", ENominal.COUNT  }
        };

    public ESuit Suit
    {
        get
        {
            //СОХРАНЯТЬ при копировании ТОЛЬКО НАСИЛЬНО и только В UTF-8
            switch (suit)
            {
                case "♥":
                    return ESuit.HEART;
                case "♦":
                    return ESuit.TILE;
                case "♣":
                    return ESuit.CLOVERS;
                case "♠":
                    return ESuit.PIKES;

                default: throw new System.Exception("Error: unknown suit");
            }
        }
    }
    public ENominal Nominal => _niminal[nominal];

}
