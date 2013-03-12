using System.Runtime.Serialization;
using Umbraco.Core.Models.EntityBase;
using Umbraco.Core.Persistence.Mappers;

namespace Umbraco.Core.Models
{
    [Mapper(typeof(DictionaryTranslationMapper))]
    public interface IDictionaryTranslation : IEntity
    {
        /// <summary>
        /// Gets or sets the <see cref="Language"/> for the translation
        /// </summary>
        [DataMember]
        ILanguage Language { get; set; }

        /// <summary>
        /// Gets or sets the translated text
        /// </summary>
        [DataMember]
        string Value { get; set; }
    }
}