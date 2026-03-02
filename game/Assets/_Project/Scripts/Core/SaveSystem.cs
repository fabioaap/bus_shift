using System;
using UnityEngine;

namespace BusShift.Core
{
    /// <summary>
    /// Save data for a single game slot.
    ///
    /// GhostsDefeated is a bitmask over the GhostType enum:
    ///   bit 0 = Marcus, bit 1 = Emma, bit 2 = Thomas, bit 3 = Grace, bit 4 = Oliver.
    /// Use <see cref="SaveData.GetGhostDefeated"/> and <see cref="SaveData.SetGhostDefeated"/>
    /// to read/write individual flags without manipulating the int directly.
    /// </summary>
    [Serializable]
    public struct SaveData
    {
        /// <summary>Current day (1-5).</summary>
        public int Day;

        /// <summary>Current period (0 = Morning, 1 = Night).</summary>
        public int Period;

        /// <summary>Total periods completed so far (0-9).</summary>
        public int TotalPeriods;

        /// <summary>Player sanity at the time of saving (0-1).</summary>
        public float Sanity;

        /// <summary>Human-readable save timestamp ("yyyy-MM-dd HH:mm:ss").</summary>
        public string SaveDateTime;

        // ── Ghost defeated flags (stored as a bitmask internally) ────────────────

        /// <summary>
        /// Bitmask for defeated ghosts this session.
        /// Bit index matches the int value of <see cref="Ghosts.GhostType"/>.
        /// </summary>
        public int GhostsDefeatedMask;

        // ── Convenience accessors ─────────────────────────────────────────────────

        /// <summary>
        /// Returns the defeated flag for the ghost at the given index (0-4).
        /// Index matches the int value of <see cref="Ghosts.GhostType"/>.
        /// </summary>
        public bool GetGhostDefeated(int ghostIndex) =>
            ghostIndex >= 0 && ghostIndex < 32 && (GhostsDefeatedMask & (1 << ghostIndex)) != 0;

        /// <summary>
        /// Sets or clears the defeated flag for the ghost at the given index (0-4).
        /// </summary>
        public void SetGhostDefeated(int ghostIndex, bool defeated)
        {
            if (ghostIndex < 0 || ghostIndex >= 32) return;
            if (defeated)
                GhostsDefeatedMask |=  (1 << ghostIndex);
            else
                GhostsDefeatedMask &= ~(1 << ghostIndex);
        }

        /// <summary>
        /// Returns a bool array of length <paramref name="count"/> (default 5) for all ghost flags.
        /// </summary>
        public bool[] GetAllGhostsDefeated(int count = 5)
        {
            var result = new bool[count];
            for (int i = 0; i < count; i++)
                result[i] = GetGhostDefeated(i);
            return result;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Lightweight save/load system backed by PlayerPrefs.
    ///
    /// Slot layout:
    ///   Slot 0 (AutoSaveSlot) — written automatically at every period end.
    ///   Slot 1, 2             — available for manual saves from the UI.
    ///
    /// Key format: "BusShift_Save_Slot{N}_{Field}"
    /// </summary>
    public static class SaveSystem
    {
        // ── Configuration ────────────────────────────────────────────────────────

        /// <summary>Total number of save slots (0, 1, 2).</summary>
        public const int MaxSlots = 3;

        /// <summary>Slot index reserved for auto-save (written on every period end).</summary>
        public const int AutoSaveSlot = 0;

        // ── Internal ─────────────────────────────────────────────────────────────

        private const string KeyPrefix = "BusShift_Save_";
        private const string ExistsKey = "Exists";

        // ── Public API ───────────────────────────────────────────────────────────

        /// <summary>
        /// Writes <paramref name="data"/> to the specified <paramref name="slot"/>.
        /// Calls <see cref="PlayerPrefs.Save"/> immediately to flush to disk.
        /// </summary>
        /// <param name="slot">Target slot index (0 to <see cref="MaxSlots"/>-1).</param>
        /// <param name="data">Data to persist.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when slot index is out of range.</exception>
        public static void SaveGame(int slot, SaveData data)
        {
            ValidateSlot(slot);
            string p = Prefix(slot);

            PlayerPrefs.SetInt(   p + "Day",          data.Day);
            PlayerPrefs.SetInt(   p + "Period",        data.Period);
            PlayerPrefs.SetInt(   p + "TotalPeriods",  data.TotalPeriods);
            PlayerPrefs.SetFloat( p + "Sanity",        data.Sanity);
            PlayerPrefs.SetInt(   p + "GhostsMask",    data.GhostsDefeatedMask);
            PlayerPrefs.SetString(p + "SaveDateTime",  data.SaveDateTime ?? string.Empty);
            PlayerPrefs.SetInt(   p + ExistsKey,       1);

            PlayerPrefs.Save();

            Debug.Log($"[SaveSystem] Slot {slot} saved → " +
                      $"Day {data.Day} {(data.Period == 0 ? "Morning" : "Night")}, " +
                      $"Sanity {data.Sanity:P0}, {data.SaveDateTime}");
        }

        /// <summary>
        /// Reads and returns the <see cref="SaveData"/> stored in <paramref name="slot"/>.
        /// Returns <c>default(SaveData)</c> and logs a warning if the slot is empty.
        /// </summary>
        /// <param name="slot">Source slot index (0 to <see cref="MaxSlots"/>-1).</param>
        public static SaveData LoadGame(int slot)
        {
            ValidateSlot(slot);

            if (!SlotExists(slot))
            {
                Debug.LogWarning($"[SaveSystem] Slot {slot} is empty — returning default SaveData.");
                return default;
            }

            string p = Prefix(slot);

            return new SaveData
            {
                Day                 = PlayerPrefs.GetInt(   p + "Day",         1),
                Period              = PlayerPrefs.GetInt(   p + "Period",       0),
                TotalPeriods        = PlayerPrefs.GetInt(   p + "TotalPeriods", 0),
                Sanity              = PlayerPrefs.GetFloat( p + "Sanity",       0f),
                GhostsDefeatedMask  = PlayerPrefs.GetInt(   p + "GhostsMask",   0),
                SaveDateTime        = PlayerPrefs.GetString(p + "SaveDateTime", string.Empty)
            };
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="slot"/> contains saved data.
        /// </summary>
        public static bool SlotExists(int slot)
        {
            ValidateSlot(slot);
            return PlayerPrefs.GetInt(Prefix(slot) + ExistsKey, 0) == 1;
        }

        /// <summary>
        /// Permanently deletes all keys belonging to <paramref name="slot"/> and flushes.
        /// </summary>
        public static void DeleteSlot(int slot)
        {
            ValidateSlot(slot);
            string p = Prefix(slot);

            PlayerPrefs.DeleteKey(p + "Day");
            PlayerPrefs.DeleteKey(p + "Period");
            PlayerPrefs.DeleteKey(p + "TotalPeriods");
            PlayerPrefs.DeleteKey(p + "Sanity");
            PlayerPrefs.DeleteKey(p + "GhostsMask");
            PlayerPrefs.DeleteKey(p + "SaveDateTime");
            PlayerPrefs.DeleteKey(p + ExistsKey);

            PlayerPrefs.Save();
            Debug.Log($"[SaveSystem] Slot {slot} deleted.");
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        private static string Prefix(int slot) => $"{KeyPrefix}Slot{slot}_";

        private static void ValidateSlot(int slot)
        {
            if (slot < 0 || slot >= MaxSlots)
                throw new ArgumentOutOfRangeException(
                    nameof(slot),
                    $"Slot must be in range [0, {MaxSlots - 1}]. Got: {slot}");
        }
    }
}
