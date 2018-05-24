﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace Kontract.Interface
{
    /// <summary>
    /// This is the font adapter interface for creating font format plugins.
    /// </summary>
    public interface IFontAdapter
    {
        /// <summary>
        /// The list of characters provided by the font adapter to the UI.
        /// </summary>
        IEnumerable<FontCharacter> Characters { get; }

        /// <summary>
        /// The list of textures provided by the font adapter to the UI.
        /// </summary>
        List<Bitmap> Textures { get; }

        /// <summary>
        /// Character baseline.
        /// </summary>
        float BaseLine { get; set; }

        /// <summary>
        /// Character descent line.
        /// </summary>
        float DescentLine { get; set; }
    }

    /// <summary>
    /// This interface allows the font adapter to add new characters through the UI.
    /// </summary>
    public interface ICanAddCharacters
    {
        /// <summary>
        /// Creates a new character and allows the plugin to provide its derived type.
        /// </summary>
        /// <returns>FontCharacter or a derived type.</returns>
        FontCharacter NewCharacter();

        /// <summary>
        /// Adds a newly created character to the file and allows the plugin to perform any required adding steps.
        /// </summary>
        /// <param name="character"></param>
        /// <returns>True if the character was added, False otherwise.</returns>
        bool AddCharacter(FontCharacter character);
    }

    /// <summary>
    /// This interface allows the font afapter to rename characters through the UI making use of the NameList.
    /// </summary>
    public interface ICanRenameCharacters
    {
        /// <summary>
        /// Renames an character and allows the plugin to perform any required renaming steps.
        /// </summary>
        /// <param name="character">The character being renamed.</param>
        /// <param name="name">The new name to be assigned.</param>
        /// <returns>True if the character was renamed, False otherwise.</returns>
        bool RenameCharacter(FontCharacter character, string name);
    }

    /// <summary>
    /// This interface allows the font adapter to delete characters through the UI.
    /// </summary>
    public interface ICanDeleteCharacters
    {
        /// <summary>
        /// Deletes an character and allows the plugin to perform any required deletion steps.
        /// </summary>
        /// <param name="character">The character to be deleted.</param>
        /// <returns>True if the character was successfully deleted, False otherwise.</returns>
        bool DeleteCharacter(FontCharacter character);
    }

    /// <summary>
    /// Characters provide an extended properties dialog?
    /// </summary>
    public interface ICharactersHaveExtendedProperties
    {
        // TODO: Figure out how to best implement this feature with WPF.
        /// <summary>
        /// Opens the extended properties dialog for an character.
        /// </summary>
        /// <param name="character">The character to view and/or edit extended properties for.</param>
        /// <returns>True if changes were made, False otherwise.</returns>
        bool ShowCharacterProperties(FontCharacter character);
    }

    /// <summary>
    /// The base character class.
    /// </summary>
    public class FontCharacter : INotifyPropertyChanged
    {
        public virtual uint Character { get; set; } = 'A';

        public virtual int TextureIndex { get; set; } = 0;

        public virtual int GlyphX { get; set; } = 0;

        public virtual int GlyphY { get; set; } = 0;

        public virtual int GlyphWidth { get; set; } = 0;

        public virtual int GlyphHeight { get; set; } = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString() => ((char)Character).ToString();
    }
}
