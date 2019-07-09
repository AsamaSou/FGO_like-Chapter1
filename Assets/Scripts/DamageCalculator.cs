using UnityEngine;

public class DamageCalculator{

	public static int DamageCalc(SelectedCard attacker, Character enemy)
    {
        // 縦横ともにセイバー、ランサー、アーチャーの順
        float[,] vs = new float[3, 3] {
            {1.0f,  2.0f,   0.5f },
            {0.5f,  1.0f,   2.0f },
            {2.0f,  0.5f,   1.0f },
        };

        Debug.Log(attacker.CharaClass.ToString()+"→"+enemy.servantClass.ToString());

        //0.23*ATK*クラス補正*クラス相性*天地人相性*乱数[0.9～1.1]*クリティカル[2.0]
        double damage;
        //bool isCritical;//未実装だけど、なんかスターとか取って、わちゃっと判定したらいいと思う。
        
        //クラス補正
        if(attacker.CharaClass == Character.クラス.セイバー)
        {
            damage = 1;
        }
        else if(attacker.CharaClass == Character.クラス.ランサー)
        {
            damage = 1.05f;
        }
        else
        {
            damage = 0.95f;
        }

        if (attacker.CharaClass == Character.クラス.バーサーカー || enemy.servantClass == Character.クラス.バーサーカー)
        {
            damage *= 1.5f;
            Debug.Log("[Burserker]");
        }
        else
        {
            //クラス相性判定
            float classJudge = vs[(int)attacker.CharaClass, (int)enemy.servantClass];
            if(classJudge == 2.0f) {
                damage *= 2;
                Debug.Log("[WEAK!!]");
            }
            else if ( classJudge == 1.0f)
            {
                damage *= 1;
                Debug.Log("[DAMAGE!]");
            }
            else
            {
                damage *= 0.5f;
                Debug.Log("[RESIST]");
            }
        }
        
        /*
        //天地人相性判定
        if ((int)attacker.Attribute - (int)enemy.tentizin == -1 ||
            (int)attacker.Attribute - (int)enemy.tentizin == 2)
        {
            damage *= 1.1f;
        }
        else if ((int)attacker.Attribute - (int)enemy.tentizin == 0)
        {
            damage *= 1;
        }
        else
        {
            damage *= 0.9f;
        }
        */

        damage *= 0.23 * attacker.Atk * UnityEngine.Random.Range(0.90f, 1.10f);
        return (int)damage;
    }
}
