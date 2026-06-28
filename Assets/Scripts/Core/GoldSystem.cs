// 职责：维护关卡内金币余额，并处理扣款规则。
namespace TowerDefense.Core
{
    public sealed class GoldSystem
    {
        public int Balance { get; private set; }

        public GoldSystem(int initialBalance)
        {
            Balance = initialBalance;
        }

        public bool Spend(int amount)
        {
            if (Balance < amount)
            {
                return false;
            }

            Balance -= amount;
            return true;
        }
    }
}
