using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Level Action", menuName = "ScriptableObjects/Create Level Action")]
public class LevelActions : ScriptableObject
{
    [Header("Required refrences")]
    public TileCreator tileCreatorPreset;

    [Header("Required refrences")]
    public Sprite lockSprite;

    public void SummonStoneTiles()
    {
        SubTileColor[] availableColors = new SubTileColor[] { SubTileColor.Stone };

        foreach (stoneTileDataStruct stoneTile in GameManager.currentLevel.stoneTiles)
        {
            Tile tile = null;

            if (stoneTile.randomValues)
            {
                tile = tileCreatorPreset.CreateTile(GameManager.returnTileTypeStone(), GameManager.currentLevel.levelAvailablesymbols, availableColors);
            }
            else
            {
                tile = tileCreatorPreset.CreateTile(GameManager.returnTileTypeStone(), stoneTile.leftTileSymbol, stoneTile.rightTileSymbol, SubTileColor.Stone, SubTileColor.Stone);
            }

            if(!tile)
            {
                Debug.LogError("Problem with stone tiles");
                return;
            }

            GameManager.gameRing.DropTileIntoCell(stoneTile.cellIndex, tile, true);
        }
    }

    public void SummonSlices()
    {
        LevelSO currentLevel = GameManager.currentLevel;

        List<sliceToSpawnDataStruct> allSlices = currentLevel.slicesToSpawn.Where(x => x.RandomSlicePositions == false).ToList();
        List<sliceToSpawnDataStruct> randomSlices = currentLevel.slicesToSpawn.Where(x => x.RandomSlicePositions == true).ToList();

        //we do this to make sure that we first summon the Specific slices.
        allSlices.AddRange(randomSlices);


        for (int i = 0; i < allSlices.Count; i++)
        {
            ConditonsData sliceData = null;

            //TEMP
            SubTileSymbol symbol = SubTileSymbol.NoShape;
            SubTileColor color = SubTileColor.NoColor;

            switch (allSlices[i].sliceToSpawn)
            {
                case SliceConditionsEnums.GeneralColor:
                    sliceData = new GeneralColorCondition();
                    break;
                case SliceConditionsEnums.GeneralSymbol:
                    sliceData = new GeneralSymbolCondition();
                    break;
                case SliceConditionsEnums.SpecificColor:
                    sliceData = new SpecificColorCondition();
                    (sliceData as SpecificColorCondition).requiredColor = currentLevel.slicesToSpawn[i].specificSlicesColor;
                    color = currentLevel.slicesToSpawn[i].specificSlicesColor;
                    break;
                case SliceConditionsEnums.SpecificSymbol:
                    sliceData = new SpecificSymbolCondition();
                    (sliceData as SpecificSymbolCondition).requiredSymbol = currentLevel.slicesToSpawn[i].specificSlicesShape;
                    symbol = currentLevel.slicesToSpawn[i].specificSlicesShape;
                    break;
                default:
                    break;
            }

            int summonIndex = -1;

            if(!allSlices[i].RandomSlicePositions)
            {
                summonIndex = allSlices[i].specificSliceIndex;
            }
            else
            {
                summonIndex = Random.Range(0, GameManager.gameRing.ringSlices.Length);
            }

            if (summonIndex > -1)
            {
                Cell sameIndexCell = GameManager.gameRing.ringCells[summonIndex];

                Cell leftNeighborCell = GetLeftOfCell(summonIndex);

                if (sliceData == null)
                {
                    Debug.LogError("No slice data accepted.");
                    return;
                }

                sameIndexCell.leftSlice.sliceData = sliceData;
                leftNeighborCell.rightSlice.sliceData = sliceData;

                //slice is the same for both cells so no need for neighbor check

                int tempInt = i;
                // we use a temp int here because of the way actions work "Variable capture"
                // "Capturing" the variable i in your lambda.
                // C# captures the VARIABLE, not the VALUE at that moment.
                // when run, the lambda uses the final post-for-loop exit value of i, which wil be beyond the index range."
                sameIndexCell.leftSlice.sliceData.onGoodConnectionActions += () => currentLevel.slicesToSpawn[tempInt].onConnectionEvents?.Invoke();
                
                if(allSlices[i].isLock)
                {
                    sameIndexCell.leftSlice.sliceData.onGoodConnectionActions += () => sameIndexCell.SetAsLocked(true);
                    leftNeighborCell.rightSlice.sliceData.onGoodConnectionActions += () => leftNeighborCell.SetAsLocked(true);
                    GameManager.gameRing.ringSlices[summonIndex].SetSprite(lockSprite);
                }


                GameManager.gameRing.ringSlices[summonIndex].InitSlice(sliceData, allSlices[i].sliceToSpawn, symbol, color, allSlices[i].isLock);


                // summon slice displays under slice transforms;
                GameManager.gameRing.SetSliceDisplay(GameManager.gameRing.ringSlices[summonIndex], summonIndex);

            }
            else
            {
                Debug.LogError("Summon index wrong?");
            }
        }



    }

    private Cell GetLeftOfCell(int index)
    {
        index -= 1;

        if (index < 0)
        {
            index = GameManager.gameRing.ringCells.Length - 1;
        }

        return GameManager.gameRing.ringCells[index];
    }
}

