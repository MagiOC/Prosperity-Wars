﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using Nashet.UnityUIUtils;
using Nashet.Utils;
namespace Nashet.EconomicSimulation
{
    public class CountryStorageTable : UITableNew
    {
        public override void Refresh()
        {
            StartUpdate();
            //lock (gameObject)
            {
                RemoveButtons();
                AddHeader();
                AddButtons();
                // AddHeader();
            }
            EndUpdate();
        }
        protected override void AddHeader()
        {
            // Adding product name 
            AddButton("Product");

            ////Adding production
            AddButton("Gov. storage");

            AddButton("Mil. needs");

            AddButton("Production tot.");

            AddButton("Used by gov.");

            AddButton("Bought by gov.");

            AddButton("Sold by gov.");

        }
        private void AddButtons()
        {
            int counter = 0;

            //do NOT rely on elements order!
            var elementsToShow = Product.getAll(x => x.isTradable()).ToList();
            var howMuchRowsShow = CalcSize(elementsToShow.Count);
            var needs = Game.Player.getRealAllNeeds();
            //foreach (var product in Product.getAll())                    
            for (int i = 0; i < howMuchRowsShow; i++)
            {
                var product = elementsToShow[i + GetRowOffset()];

                // Adding product name 
                if (product.isAbstract())
                {
                    AddButton(product.getName() + " total", null, () => product.getSubstitutes().ToList().getString(" or "));

                    ////Adding total amount
                    AddButton(Game.Player.countryStorageSet.getTotal(product).get().ToString());

                    ////Adding mil. needs
                    AddButton(needs.getStorage(product).ToStringWithoutSubstitutes(), null, () => "That doesn't include non-abstract needs");
                    //AddButton(needs.getStorageIncludingSubstitutes(product).get().ToString());
                    //AddButton("-");

                    ////Adding Produced total
                    AddButton(Game.Player.getProducedTotalIncludingSubstitutes(product).get().ToString());
                    //, null , () => Game.Player.getWorldProductionShare(product) + " of world production");
                    // can't add statistic about share of abstract product due to Market.GetProduction can't in abstract products

                    ////Adding used by gov.
                    AddButton(Game.Player.countryStorageSet.used.getTotal(product).get().ToString());

                    ////Adding bought
                    AddButton(Game.Player.getConsumedInMarket().getTotal(product).get().ToString());

                    ////Adding Sold
                    //AddButton(Game.Player.getSentToMarketIncludingSubstituts(product).get().ToString());
                    AddButton("-");
                }
                else
                {
                    var storage = Game.Player.countryStorageSet.getFirstStorage(product);

                    // Adding product name 
                    AddButton(product.getName(), storage);

                    ////Adding storage amount
                    AddButton(storage.get().ToString(), storage);

                    ////Adding mil. needs
                    AddButton(needs.getStorage(product).get().ToString(), storage);

                    ////Adding Produced
                    AddButton(Game.Player.getProducedTotal(product).get().ToString(), storage, () => Game.Player.getWorldProductionShare(product) + " of world production");

                    ////Adding used by gov.
                    AddButton(Game.Player.countryStorageSet.used.getFirstStorage(product).get().ToString(), storage);

                    ////Adding bought
                    AddButton(Game.Player.getConsumedInMarket().getFirstStorage(product).get().ToString(), storage);

                    ////Adding Sold
                    //// finding actually sold from sentToMarket
                    //var str = Game.Player.getSentToMarket(product);
                    //var DSB = Game.market.getDemandSupplyBalance(product);
                    //if (DSB.GetHashCode() == Options.MarketInfiniteDSB.GetHashCode())
                    //    str.setZero();
                    //else
                    //    str.multiply(DSB);
                    AddButton(Game.Player.getSoldByGovernment(product).get().ToString(), storage, () => "Actually sold according to demand\nCould be less than sent to market");
                }

                counter++;
                //contentPanel.r                    
            }
        }
    }
}