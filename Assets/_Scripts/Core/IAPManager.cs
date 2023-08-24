using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour
{
    public static event Action OnPremiumPurchase;

    public string premiumProductId = "com.strephStudio.amusingTetris.premium";
    public void OnPurchaseComplete(Product product)
    {
        if(product.definition.id == premiumProductId)
        {
            Debug.Log($"{product.definition.id} Purchase completed");
            PlayerPrefsHelper.SetPremiumState(true);
            OnPremiumPurchase?.Invoke();
        }
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription reason)
    {
        if(product.availableToPurchase)
        Debug.Log($"Product purchase failed of product: {product.definition.id}. Reason:{reason.message}");
    }
}
