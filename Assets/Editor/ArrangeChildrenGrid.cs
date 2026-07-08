using UnityEngine;
using UnityEditor;

public class ArrangeChildrenGrid
{
    [MenuItem("Tools/Arrange Selected Children (Grid)")]
    static void ArrangeGrid()
    {
        GameObject area = Selection.activeGameObject;
        if (area == null)
        {
            Debug.LogError("Pilih dulu objek parent-nya (Barbie_Area) di Hierarchy, baru jalankan menu ini.");
            return;
        }

        int childCount = area.transform.childCount;
        if (childCount == 0)
        {
            Debug.LogWarning("Objek ini nggak punya child.");
            return;
        }

        int columns = Mathf.CeilToInt(Mathf.Sqrt(childCount));
        int rows = Mathf.CeilToInt((float)childCount / columns);
        float spacing = 115f; // jarak antar objek, tinggal ubah angka ini kalau masih numpuk

        int i = 0;
        foreach (Transform child in area.transform)
        {
            int row = i / columns;
            int col = i % columns;
            float x = (col - (columns - 1) / 2f) * spacing;
            float z = (row - (rows - 1) / 2f) * spacing;
            child.localPosition = new Vector3(x, 0, z);
            i++;
        }

        Debug.Log("Selesai! " + childCount + " objek diatur jadi grid.");
    }
}