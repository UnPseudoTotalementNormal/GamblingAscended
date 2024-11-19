using UnityEngine;

public interface GL_ICoinHolder
{
    public float MoneyInserted { get; }
    
    public void AddMoney(float value);
    public void RemoveMoney(float value);
}
