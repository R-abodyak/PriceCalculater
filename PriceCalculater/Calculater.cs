class Calculater { 
    public decimal Calculate(decimal price , decimal percentage)
    {
        return (percentage / 100) * price;
    }

    public decimal ApplyPrecision(decimal price)
    {
        return Math.Round(price, 2);
    }


}

