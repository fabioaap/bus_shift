using System;
using UnityEngine;

namespace BusShift.Core
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Shared data types for the narrative pipeline.
    //  Designers and writers fill these in via the Unity Inspector.
    //
    //  Quick-start:
    //    • Right-click in the Project window → BusShift/Narrative/Dialogue Sequence
    //    • Right-click in the Project window → BusShift/Narrative/Cutscene Sequence
    //    • Place DialogueSequence assets in:  Resources/DialogueSequences/
    //    • Place CutsceneSequence assets in:  Resources/CutsceneSequences/
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// One spoken line within a radio dialogue sequence.
    ///
    /// <para>
    /// <b>SpeakerId</b> is used as a bold prefix in the subtitle, e.g.
    /// <c>&lt;b&gt;Earl Hayes:&lt;/b&gt; "Are you still out there, Morgan?"</c>
    /// </para>
    /// </summary>
    [Serializable]
    public struct DialogueLine
    {
        /// <summary>
        /// Character identifier shown in the subtitle.
        /// Examples: <c>"Earl Hayes"</c>, <c>"Ray Morgan"</c>.
        /// Leave empty to show text without a speaker prefix.
        /// </summary>
        [Tooltip("Character name shown as subtitle prefix (e.g. 'Earl Hayes'). Leave empty for no prefix.")]
        public string SpeakerId;

        /// <summary>The subtitle text displayed for this line.</summary>
        [TextArea(2, 5)]
        [Tooltip("Subtitle text displayed on screen while this line plays.")]
        public string Text;

        /// <summary>
        /// How many seconds the subtitle remains on screen
        /// (including fade-in and fade-out time in <see cref="DialogueSystem"/>).
        /// </summary>
        [Tooltip("Seconds the subtitle is visible (includes fade in/out).")]
        [Min(0.1f)]
        public float Duration;

        /// <summary>Optional voice-over clip played when this line starts.</summary>
        [Tooltip("Optional voice-over audio clip (played via AudioManager.PlaySFX).")]
        public AudioClip VoiceClip;
    }

    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Data asset that defines an ordered radio dialogue sequence between
    /// Earl Hayes (dispatcher) and Ray Morgan (driver).
    ///
    /// <para>
    /// Loaded at runtime by <see cref="DialogueSystem.PlayRadioDialogue"/>
    /// using <c>Resources.Load&lt;DialogueSequence&gt;("DialogueSequences/{SequenceId}")</c>.
    /// </para>
    ///
    /// <para>
    /// <b>For writers:</b> create a new asset via
    /// <i>Right-click → BusShift/Narrative/Dialogue Sequence</i> and fill in
    /// <see cref="SequenceId"/> (must match the asset filename) and <see cref="Lines"/>.
    /// </para>
    /// </summary>
    [CreateAssetMenu(
        fileName = "NewDialogueSequence",
        menuName = "BusShift/Narrative/Dialogue Sequence",
        order    = 10)]
    public class DialogueSequence : ScriptableObject
    {
        /// <summary>
        /// Unique string ID used by <see cref="DialogueSystem.PlayRadioDialogue"/>.
        /// Must match the asset filename exactly (case-sensitive).
        /// Example: <c>"Day1IntroSequence"</c>.
        /// </summary>
        [Tooltip("Must match the asset filename exactly. Used by DialogueSystem.PlayRadioDialogue(id).")]
        public string SequenceId;

        /// <summary>Ordered array of lines played from index 0 to the last element.</summary>
        [Tooltip("Lines played in order from top to bottom.")]
        public DialogueLine[] Lines;
    }

    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Data asset that defines a cinematic cutscene sequence.
    ///
    /// <para>
    /// Loaded at runtime by <see cref="CutsceneManager.PlayCutscene"/>
    /// using <c>Resources.Load&lt;CutsceneSequence&gt;("CutsceneSequences/{id}")</c>.
    /// </para>
    ///
    /// <para>
    /// <b>For designers:</b> create a new asset via
    /// <i>Right-click → BusShift/Narrative/Cutscene Sequence</i>, fill in the
    /// fields, and name the asset exactly as the matching <see cref="CutsceneId"/>
    /// value (e.g. <c>Day1Intro</c>).
    /// </para>
    ///
    /// <para>
    /// <b>Camera note:</b> <see cref="CameraPositions"/> accepts prefab-rooted
    /// <c>Transform</c> references (e.g. Cinemachine Virtual Camera prefabs or
    /// cinematic dolly rail anchors).  If left empty the active gameplay camera
    /// is used as-is.
    /// </para>
    /// </summary>
    [CreateAssetMenu(
        fileName = "NewCutsceneSequence",
        menuName = "BusShift/Narrative/Cutscene Sequence",
        order    = 11)]
    public class CutsceneSequence : ScriptableObject
    {
        /// <summary>
        /// Maximum wall-clock duration of this cutscene in seconds.
        /// When <see cref="DialogueSequenceId"/> is set, the cutscene ends as
        /// soon as the dialogue completes OR this timeout is reached —
        /// whichever comes first.
        /// When there is no dialogue, the cutscene simply waits this long.
        /// </summary>
        [Tooltip("Max cutscene duration in seconds. Acts as timeout when dialogue is present.")]
        [Min(0f)]
        public float Duration = 5f;

        /// <summary>
        /// Optional ordered list of camera anchor <c>Transform</c>s the cutscene
        /// will visit.  Each entry should be a prefab-based transform reference
        /// (e.g. a Cinemachine Virtual Camera).
        /// Leave empty to keep the current active camera unchanged.
        /// </summary>
        [Tooltip("Optional camera anchor transforms visited during the cutscene. Empty = use current camera.")]
        public Transform[] CameraPositions;

        /// <summary>
        /// Optional <see cref="DialogueSequence.SequenceId"/> to trigger via
        /// <see cref="DialogueSystem.PlayRadioDialogue"/> when the cutscene starts.
        /// Leave empty for a silent (no-dialogue) cutscene.
        /// </summary>
        [Tooltip("Optional dialogue SequenceId to play. Leave empty for a silent cutscene.")]
        public string DialogueSequenceId;

        /// <summary>
        /// When <c>true</c>, <see cref="CutsceneManager"/> will fade the screen
        /// from black to visible at the start of this cutscene.
        /// </summary>
        [Tooltip("Fade in from black when the cutscene starts.")]
        public bool FadeIn = true;

        /// <summary>
        /// When <c>true</c>, <see cref="CutsceneManager"/> will fade the screen
        /// to black at the end of this cutscene.
        /// </summary>
        [Tooltip("Fade out to black when the cutscene ends.")]
        public bool FadeOut = true;
    }
}
