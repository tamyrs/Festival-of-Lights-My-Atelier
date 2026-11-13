using System;
using System.Collections.Generic;

namespace FestivalOfLights
{
    /// <summary>
    /// Representa uma encomenda de lanterna de papel, conforme o GDD (seção 3.1).
    /// Cada pedido especifica formato, cor e estampa desejados pelo cliente.
    /// </summary>
    [Serializable]
    public class LanternOrder
    {
        public string customerName;
        public LanternFormat format;
        public LanternColor color;
        public LanternPattern pattern;
        public int rewardCoins;

        public LanternOrder(string customerName, LanternFormat format, LanternColor color,
            LanternPattern pattern, int rewardCoins)
        {
            this.customerName = customerName;
            this.format = format;
            this.color = color;
            this.pattern = pattern;
            this.rewardCoins = rewardCoins;
        }

        public override string ToString()
        {
            return $"{customerName} quer uma lanterna {format} {color} com estampa de {pattern} ({rewardCoins} moedas)";
        }
    }

    public enum LanternFormat { Round, Cylinder, Square }
    public enum LanternColor { Red, Yellow, Blue, Green, Pink }
    public enum LanternPattern { Flowers, Stars, Moon }

    /// <summary>
    /// Lista predefinida de pedidos (sem geração procedural complexa, conforme GDD 3.1).
    /// </summary>
    public static class OrderDatabase
    {
        public static readonly List<LanternOrder> Orders = new List<LanternOrder>
        {
            new LanternOrder("Sr. Pereira", LanternFormat.Round, LanternColor.Red, LanternPattern.Flowers, 20),
            new LanternOrder("Dona Clara", LanternFormat.Cylinder, LanternColor.Yellow, LanternPattern.Stars, 25),
            new LanternOrder("Joãozinho", LanternFormat.Square, LanternColor.Blue, LanternPattern.Moon, 30),
            new LanternOrder("Vovó Lúcia", LanternFormat.Round, LanternColor.Pink, LanternPattern.Flowers, 20),
            new LanternOrder("Sr. Gomes", LanternFormat.Cylinder, LanternColor.Green, LanternPattern.Stars, 25),
            new LanternOrder("Mariana", LanternFormat.Square, LanternColor.Red, LanternPattern.Moon, 30),
            new LanternOrder("Pedro", LanternFormat.Round, LanternColor.Blue, LanternPattern.Stars, 25),
            new LanternOrder("Ana", LanternFormat.Cylinder, LanternColor.Pink, LanternPattern.Moon, 30),
            new LanternOrder("Carlos", LanternFormat.Square, LanternColor.Yellow, LanternPattern.Flowers, 20),
            new LanternOrder("Beatriz", LanternFormat.Round, LanternColor.Green, LanternPattern.Stars, 25),
        };
    }
}
