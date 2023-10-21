using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtensions
{
    public static void FillArrayOnes(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = 1;
        }
    }

    public static void FillArraySequence(int[] array,int start = 0)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = start + i;
        }
    }
}
