using System.Collections.Generic;

public class BarcodeDataDTO
{
    public List<ItemCreationDTO> Items { get; set; }
    public BarcodeDataDTO(List<string> barcodes, List<int> amountCounted, int shelfOfOrigin) 
    {
        Items = new List<ItemCreationDTO>();

        for (int i = 0; i < barcodes.Count; i++)
        {
            Items.Add(new ItemCreationDTO
            {
                AmountCounted = amountCounted[i],
                ShelfOfOrigin = shelfOfOrigin,
                Barcode = barcodes[i]
            });
        }
    }
}
