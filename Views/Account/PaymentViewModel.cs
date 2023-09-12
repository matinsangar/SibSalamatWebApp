using System;
using System.Collections.Generic;
using SibSalamat.Models;

namespace SibSalamat.Views.Account;

public class PaymentViewModel
{
    public string UserName { get; set; }
    public List<Sell> SalesHistory { get; set; }
}