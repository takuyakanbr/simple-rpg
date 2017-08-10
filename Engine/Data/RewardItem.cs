using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Data
{
    public class RewardItem
    {
        public Item Data;
        public int Quantity;

        public RewardItem(Item data, int quantity)
        {
            Data = data;
            Quantity = quantity;
        }
    }
}
