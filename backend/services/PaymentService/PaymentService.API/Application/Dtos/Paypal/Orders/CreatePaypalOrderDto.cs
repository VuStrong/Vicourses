﻿namespace PaymentService.API.Application.Dtos.Paypal.Orders
{
    public class CreatePaypalOrderDto
    {
        public required string Intent { get; set; }
        public required List<PaypalPurchaseUnitDto> PurchaseUnits { get; set; }
    }
}
