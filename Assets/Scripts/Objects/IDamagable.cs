public interface IDamageable
{
    bool isDamaged();
    void Damage();
}

public interface IRepairable
{
    bool isDamaged();
    void Repair();
}