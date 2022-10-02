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

    
    
    int currentSummonIndex;
    int summonedSliceCount = 0;
    List<Slice> tempSlicesList;
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
        summonedSliceCount = 0;
        currentSummonIndex = -1;

        LevelSO currentLevel = GameManager.currentLevel;

        //List<sliceToSpawnDataStruct> allSlices = currentLevel.slicesToSpawn.Where(x => x.RandomSlicePositions == false).ToList();
        //List<sliceToSpawnDataStruct> randomSlices = currentLevel.slicesToSpawn.Where(x => x.RandomSlicePositions == true).ToList();

        List<sliceToSpawnDataStruct> allSlices = currentLevel.slicesToSpawn.ToList();

        tempSlicesList = new List<Slice>();
        tempSlicesList.AddRange(GameManager.gameRing.ringSlices);

        //we do this to make sure that we first summon the Specific slices.
        //allSlices.AddRange(randomSlices);


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

                    if(allSlices[i].RandomSliceValues)
                    {
                        int randomIndex = Random.Range(0, GameManager.currentLevel.levelAvailableColors.Length);

                        (sliceData as SpecificColorCondition).requiredColor = GameManager.currentLevel.levelAvailableColors[randomIndex];
                        color = GameManager.currentLevel.levelAvailableColors[randomIndex];
                    }
                    else
                    {
                        (sliceData as SpecificColorCondition).requiredColor = currentLevel.slicesToSpawn[i].specificSlicesColor;
                        color = currentLevel.slicesToSpawn[i].specificSlicesColor;
                    }
                    break;
                case SliceConditionsEnums.SpecificSymbol:
                    sliceData = new SpecificSymbolCondition();

                    if (allSlices[i].RandomSliceValues)
                    {
                        int randomIndex = Random.Range(0, GameManager.currentLevel.levelAvailablesymbols.Length);

                        (sliceData as SpecificSymbolCondition).requiredSymbol = GameManager.currentLevel.levelAvailablesymbols[randomIndex];
                        symbol = GameManager.currentLevel.levelAvailablesymbols[randomIndex];
                    }
                    else
                    {
                        (sliceData as SpecificSymbolCondition).requiredSymbol = currentLevel.slicesToSpawn[i].specificSlicesShape;
                        symbol = currentLevel.slicesToSpawn[i].specificSlicesShape;
                    }

                    break;
                default:
                    break;
            }

            //if(!allSlices[i].RandomSlicePositions)
            //{
            //    summonIndex = allSlices[i].specificSliceIndex;
            //}
            //else
            //{
            //    summonIndex = Random.Range(0, GameManager.gameRing.ringSlices.Length);
            //}

            if (!GameManager.currentLevel.isRandomSlicePositions)
            {
                currentSummonIndex = allSlices[i].specificSliceIndex;
            }
            else
            {
                if (summonedSliceCount > 0)
                {
                    currentSummonIndex += ReturnSliceSummonIndex();

                    if (currentSummonIndex >= GameManager.gameRing.ringSlices.Length)
                    {
                        currentSummonIndex -= GameManager.gameRing.ringSlices.Length;
                    }

                    if (currentSummonIndex < 0)
                    {
                        currentSummonIndex *= -1;
                    }

                }
                else
                {
                    // first summon is always random
                    currentSummonIndex = Random.Range(0, GameManager.gameRing.ringSlices.Length);
                }
            }

            Debug.Log(currentSummonIndex);

            if (currentSummonIndex > -1)
            {
                if (!GameManager.gameRing.ringSlices[currentSummonIndex].CheckHasSlideData())
                {
                    Debug.LogError("Tried to summon on exsisting slice");
                    return;
                }

                Cell sameIndexCell = GameManager.gameRing.ringCells[currentSummonIndex];

                Cell leftNeighborCell = GetLeftOfCell(currentSummonIndex);

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
                    GameManager.gameRing.ringSlices[currentSummonIndex].SetSprite(lockSprite);
                }


                GameManager.gameRing.ringSlices[currentSummonIndex].InitSlice(sliceData, allSlices[i].sliceToSpawn, symbol, color, allSlices[i].isLock);


                // summon slice displays under slice transforms;
                GameManager.gameRing.SetSliceDisplay(GameManager.gameRing.ringSlices[currentSummonIndex], currentSummonIndex);

                summonedSliceCount++;
                tempSlicesList.Remove(GameManager.gameRing.ringSlices[currentSummonIndex]);
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

    private int ReturnSliceSummonIndex()
    {
        int spacing = -1;

        if (GameManager.currentLevel.slicesToSpawn.Length == 0)
        {
            Debug.LogError("Tried to summon 0 sices");
            return -1;
        }

        switch (GameManager.currentLevel.ringType)
        {
            case Ringtype.ring8:
                return Ring8SlicesAlgo();
            case Ringtype.ring12:
                return Ring12SlicesAlgo();
            case Ringtype.NoType:
                break;
            default:
                break;
        }
        return spacing;
    }

    private int Ring8SlicesAlgo()
    {
        int spacing = -1;

        if (GameManager.currentLevel.slicesToSpawn.Length == 2)
        {
            spacing = 4;
        }
        else if (GameManager.currentLevel.slicesToSpawn.Length == 3)
        {
            spacing = 3;
        }
        else if (GameManager.currentLevel.slicesToSpawn.Length == 4)
        {
            spacing = 2;
        }
        else
        {
            if(summonedSliceCount > 3)
            {
                spacing = FindEmptyIndexSliceSlot();
            }
            else
            {
                spacing = 2;
            }
        }

        return spacing;
    }
    private int Ring12SlicesAlgo()
    {
        int spacing = -1;

        if (GameManager.currentLevel.slicesToSpawn.Length == 2)
        {
            spacing = 6;
        }
        else if (GameManager.currentLevel.slicesToSpawn.Length == 3)
        {
            spacing = 4;
        }
        else if (GameManager.currentLevel.slicesToSpawn.Length == 4)
        {
            spacing = 3;
        }
        else if(GameManager.currentLevel.slicesToSpawn.Length == 5)
        {
            if (summonedSliceCount > 2 && summonedSliceCount < 5)
            {
                spacing = 2;
            }
            else
            {
                spacing = 3;
            }
        }
        else
        {
            if (summonedSliceCount > 5)
            {
                spacing = FindEmptyIndexSliceSlot();
            }
            else
            {
                spacing = 2;
            }
        }
        return spacing;
    }

    private int FindEmptyIndexSliceSlot()
    {
        int randomNum = Random.Range(0, tempSlicesList.Count());
        int chosenSliceIndex = tempSlicesList[randomNum].index;

        Debug.Log("Random: " + chosenSliceIndex);

        int spacing = chosenSliceIndex - currentSummonIndex;

        //if (chosenSliceIndex < currentSummonIndex)
        //{
        //    spacing = chosenSliceIndex - currentSummonIndex;
        //}
        //else
        //{
        //    spacing = currentSummonIndex + chosenSliceIndex;
        //}

        //if (spacing < 0)
        //{
        //    spacing = Mathf.Abs(currentSummonIndex - chosenSliceIndex);
        //}

        return spacing;
    }
}

