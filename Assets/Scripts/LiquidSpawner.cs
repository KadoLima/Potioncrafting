using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawns;
    [SerializeField] int minBurstAmount = 25;
    [SerializeField] int maxBurstAmount = 50;
    [SerializeField] float spawnRate = 5f;
    LiquidColor liquid, previousLiquid = LiquidColor.NULL;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLiquidCoroutine());
    }

    public GameObject ChosenLiquid()
    {


        while (liquid == previousLiquid)
            liquid = ColorsManager.instance.AllColors[Random.Range(0, ColorsManager.instance.AllColors.Length)].liquidColor;

        if (ColorsManager.instance.GetClassByLiquidColor(liquid).prefab)
        {
            previousLiquid = liquid;
            return ColorsManager.instance.GetClassByLiquidColor(liquid).prefab;
        }

        Debug.LogWarning("NOT FOUND!!!!!!!!!!!!!");
        return null;
    }

    IEnumerator SpawnLiquidCoroutine()
    {
        //yield return new WaitUntil(() => QuestsManager.instance.CurrentQuest!=null  && QuestsManager.instance.CurrentQuest.questObjective != QuestObjective.NULL);
        yield return new WaitUntil(() => QuestsManager.instance.CurrentQuest != null);
        yield return new WaitForSeconds(1f);

        while (true)
        {
            yield return new WaitUntil(() => QuestsManager.instance.CurrentQuest != null);

            GameObject chosenLiquid = ChosenLiquid();
            int burstAmount = Random.Range(minBurstAmount, maxBurstAmount + 1);
            int count = 0;

            MoneyManager.instance.SpendMoney();

            while (count < burstAmount)
            {
                Instantiate(chosenLiquid, spawns[Random.Range(0,spawns.Length)].localPosition, Quaternion.identity);
                count++;
                yield return new WaitForSeconds(0.01f);
            }


            yield return new WaitForSeconds(spawnRate);
        }
    }
}
