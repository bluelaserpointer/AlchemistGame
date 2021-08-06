using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ChemicalSummonEditor
{
    /// <summary>
    /// ��ȡ���ʱ��Զ�����ScriptableObject
    /// </summary>
    [MenuItem("ChemicalSummon/LoadSubstanceExcel")]
    private static void LoadSubstanceExcel()
    {
        FileStream fileStream;
        try
        {
            fileStream = File.Open(Application.streamingAssetsPath + "/Substance.xlsx", FileMode.Open, FileAccess.Read);
        }
        catch (IOException)
        {
            Debug.LogError("Load Excel failed. Close any application opening the Excel file.");
            return;
        }
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        DataSet result = excelDataReader.AsDataSet();
        int newCreatedCount = 0;
        int updatedCount = 0;
        foreach (DataTable table in result.Tables)
        {
            bool isFirstLine = true;
            foreach(DataRow row in table.Rows)
            {
                if(isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }
                string substanceName = row[0].ToString();
                Substance substance = Substance.GetByName(substanceName);
                bool newCreated = substance == null;
                if (newCreated)
                {
                    substance = ScriptableObject.CreateInstance<Substance>();
                }
                substance.chemicalSymbol = substanceName;
                //analyze compounds from molecular name
                string molecularStr = row[1].ToString();
                if (molecularStr.Length == 0) //when structual name and molecular name are same
                    molecularStr = substanceName;
                string tmpElementName = "";
                string lastLetter = "";
                bool lastIsNumber = false;
                foreach (char letter in molecularStr)
                {
                    if (lastLetter.Length == 0)
                    {
                        lastLetter += letter;
                    }
                    else if (char.IsUpper(letter))
                    {
                        if (!lastIsNumber) // exp. CO
                        {
                            substance.PutElementAndAmount(Element.GetByName(lastLetter), 1);
                        }
                        else //exp. H2O
                        {
                            substance.PutElementAndAmount(Element.GetByName(tmpElementName), ToInt(lastLetter));
                        }
                        lastLetter = letter.ToString();
                        lastIsNumber = false;
                    }
                    else if (char.IsNumber(letter))
                    {
                        if (!lastIsNumber) // exp. H2O
                        {
                            tmpElementName = lastLetter;
                            lastLetter = letter.ToString();
                        }
                        else
                        {
                            lastLetter += letter;
                        }
                        lastIsNumber = true;
                    }
                    else if (char.IsLower(letter))
                    {
                        lastLetter += letter;
                    }
                    else
                    {
                        Debug.Log("encounted unknown character: " + letter);
                    }
                }
                //end phase
                if (!lastIsNumber) // exp. Fe
                {
                    substance.PutElementAndAmount(Element.GetByName(lastLetter), 1);
                }
                else //exp. H2
                {
                    substance.PutElementAndAmount(Element.GetByName(tmpElementName), ToInt(lastLetter));
                }
                substance.atk = ToInt(row[2].ToString());
                substance.meltingPoint = ToInt(row[3].ToString());
                substance.boilingPoint = ToInt(row[4].ToString());
                substance.name.defaultString = substanceName;
                substance.name.PutSentence_EmptyStrMeansRemove(Language.Chinese, row[5].ToString());
                substance.name.PutSentence_EmptyStrMeansRemove(Language.Japanese, row[6].ToString());
                substance.name.PutSentence_EmptyStrMeansRemove(Language.English, row[7].ToString());
                substance.description.defaultString = "";
                substance.description.PutSentence_EmptyStrMeansRemove(Language.Chinese, row[8].ToString());
                substance.description.PutSentence_EmptyStrMeansRemove(Language.Japanese, row[9].ToString());
                substance.description.PutSentence_EmptyStrMeansRemove(Language.English, row[10].ToString());
                substance.image = Resources.Load<Sprite>("Chemical/Sprites/" + substanceName);
                if (newCreated)
                {
                    AssetDatabase.CreateAsset(substance, @"Assets/GameContents/Resources/Chemical/Substance/" + substanceName + ".asset");
                    AssetDatabase.SaveAssets(); //�洢��Դ
                    ++newCreatedCount;
                }
                else
                {
                    ++updatedCount;
                }
            }
        }
        AssetDatabase.Refresh(); //ˢ��
        Debug.Log("SubstanceAssetsCreated. updatedCount: " + updatedCount + ", newCreated: " + newCreatedCount);
    }
    /// <summary>
    /// ��ȡԪ�ر��Զ�����ScriptableObject
    /// </summary>
    /// <returns></returns>
    [MenuItem("ChemicalSummon/LoadElementExcel")]
    private static void LoadElementExcel()
    {
        FileStream fileStream;
        try
        {
            fileStream = File.Open(Application.streamingAssetsPath + "/Element.xlsx", FileMode.Open, FileAccess.Read);
        }
        catch (IOException)
        {
            Debug.LogError("Load Excel failed. Close any application opening the Excel file.");
            return;
        }
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        DataSet result = excelDataReader.AsDataSet();
        DataTable table = result.Tables[0];
        int rows = table.Rows.Count;
        int newCreatedCount = 0;
        int updatedCount = 0;
        for (int row = 1; row < rows; row++)
        {
            DataRow rowData = table.Rows[row];
            string elementName = rowData[0].ToString();
            Element element = Element.GetByName(elementName);
            bool newCreated = element == null;
            if (newCreated)
            {
                element = ScriptableObject.CreateInstance<Element>();
            }
            element.chemicalSymbol = elementName;
            element.atom = ToInt(rowData[1].ToString());
            element.mol = ToInt(rowData[2].ToString());
            element.name.defaultString = elementName;
            element.name.PutSentence_EmptyStrMeansRemove(Language.Chinese, rowData[3].ToString());
            element.name.PutSentence_EmptyStrMeansRemove(Language.Japanese, rowData[4].ToString());
            element.name.PutSentence_EmptyStrMeansRemove(Language.English, rowData[5].ToString());
            if(newCreated)
            {
                AssetDatabase.CreateAsset(element, @"Assets/GameContents/Resources/Chemical/Element/" + elementName + ".asset");
                AssetDatabase.SaveAssets(); //�洢��Դ
                ++newCreatedCount;
            }
            else
            {
                ++updatedCount;
            }
        }
        AssetDatabase.Refresh(); //ˢ��
        Debug.Log("ElementAssetsCreated. updatedCount: " + updatedCount + ", newCreated: " + newCreatedCount);
    }
    private static int ToInt(string str)
    {
        if (str.Length == 0)
            return 0;
        try
        {
            return Convert.ToInt32(str);
        }
        catch(FormatException)
        {
            Debug.LogWarning(str + " is not a number.");
            return 0;
        }
    }
}
