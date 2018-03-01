﻿using System;
using Godot;

public class GUIInventorySlotArray : GUIComplexObject
{
    public GUIInventorySlot ExchangeSlot { get; private set; }
    public Item.Type SlotType { get; private set; }
    public IntVector2 Dimensions { get; private set; }

    private GUIInventorySlot[,] slots;

    public Vector2 SlotSpacing { get; private set; }
    public Vector2 Offset { get; private set; }

    public GUIInventorySlotArray(GUIInventorySlot exchangeSlot, Item.Type type, 
        IntVector2 size, Vector2 slotSpacing)
    {
        this.ExchangeSlot = exchangeSlot;
        this.SlotType = type;
        this.Dimensions = size;

        this.slots = new GUIInventorySlot[Dimensions.x, Dimensions.y];

        this.SlotSpacing = slotSpacing;

        PositionSlots();
    }

    public void SaveToInventory(Inventory inv)
    {
        int inventorySize = inv.size;
        int slotNum = Dimensions.x * Dimensions.y;

        if (inventorySize < slotNum)
        {
            throw new ArgumentException("Too many items in the slot array, would overflow the given inventory");
        }

        for (int i = 0; i < slotNum; ++i)
        {
            inv.RemoveItemStack(i);
            inv.TryAddItemStack(this[i].GetCurItemStack(), i);
        }
        for (int i = slotNum; i < inventorySize; ++i)
        {
            inv.RemoveItemStack(i);
        }
    }

    internal void SetSize(Vector2 slotSpacing)
    {
        this.SlotSpacing = slotSpacing;
        HandleResize();
    }

    public void OverrideFromInventory(Inventory inv)
    {
        int inventorySize = inv.size;
        int slotNum = Dimensions.x * Dimensions.y;

        if (inventorySize > slotNum)
        {
            throw new ArgumentException("Too many items in the given inventory, cannot assign to slots");
        }

        for (int i = 0; i < inventorySize; ++i)
        {
            this[i].AssignItemStack(inv.GetItemStack(i));
        }
    }

    public void PositionSlots()
    {
        for (int x = Dimensions.x - 1; x >= 0 ; x--)
        {
            for (int y = Dimensions.y - 1; y >= 0; y--)
            {
                Vector2 pos = new Vector2(x, y) * (GUIInventorySlot.SIZE + this.SlotSpacing) + GUIInventorySlot.SIZE / 2;

                int ind = x + y * Dimensions.x;

                Vector2 slotPos = pos - GUIInventorySlot.SIZE / 2;
                ItemStack iStack = this[x, y]?.GetCurItemStack();
                this[x, y] = new GUIInventorySlot(this.ExchangeSlot, this.SlotType, ind, slotPos)
                {
                    ZAsRelative = true,
                    ZIndex = 1
                };
                this[x, y].AssignItemStack(iStack);
            }
        }
    }

    public override void HandleResize()
    {
        PositionSlots();
    }

    public GUIInventorySlot this[int x, int y]
    {
        get
        {
            return this.slots[x, y];
        }
        set
        {
            if (this.slots[x, y] != null)
            {
                this.RemoveChild(this.slots[x, y]);
            }
            this.slots[x, y] = value;
            this.AddChild(value);

        }
    }

    public GUIInventorySlot this[int i]
    {
        get
        {
            return this[i % Dimensions.x, i /Dimensions.x];
        }
        set
        {
            this[i % Dimensions.x, i / Dimensions.x] = value;
        }
    }

    public GUIInventorySlot this[IntVector2 pos]
    {
        get
        {
            return this[pos.x, pos.y];
        }
        set
        {
            this[pos.x, pos.y] = value;
        }
    }
}