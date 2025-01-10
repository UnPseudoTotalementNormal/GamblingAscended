using Enums;

public static class GL_DamageProcessor
{
        public class DamageProcessResult
        {
                public float Amount;
        }
        
        public static DamageProcessResult GetFinalDamageAmount(GL_DamageInfo damageInfo, DamageType immuneToType)
        {
                var damageResult = new DamageProcessResult();
                damageResult.Amount = damageInfo.Amount;
                
                
                //immunity calculs
                if (immuneToType == DamageType.Aucun)
                {
                        return damageResult;
                }

                if (damageInfo.DamageType == immuneToType)
                {
                        damageResult.Amount = 0;
                        return damageResult;
                }
                
                return damageResult;
        }
}