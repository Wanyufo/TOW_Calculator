using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameMaster
{
    /// <summary>
    ///     Custom Tag Manager because the existing Unity tag system does not meet the requirements. Together with the tagger
    ///     script, this turns any Enum into tags
    /// </summary>
    public class TagManager : MonoBehaviour, IManager
    {
        // #####=== Tag Enums ===#####
        /// <summary>
        ///     Specific type of tag.
        /// </summary>
        public enum SimpleTag
        {
            SimpleTag1,
            SimpleTag2,
            SimpleTag3
        }


        // #####=== Do not touch below this ===#####

        /// <summary>
        ///     Dictionary of all List for each TagType
        /// </summary>
        private Dictionary<string, List<ITag>> Tags { get; } = new();

        public void RegisterTag<TEnum>(TEnum smartTag, ITag iTag) where TEnum : Enum
        {
            var tagGroup = typeof(TEnum).ToString();
            if (!Tags.ContainsKey(tagGroup)) Tags.Add(tagGroup, new List<ITag>());

            Tags[tagGroup].Add(iTag);
        }

        public void DeRegisterTag<TEnum>(TEnum smartTag, ITag iTag) where TEnum : Enum
        {
            Tags[typeof(TEnum).ToString()].Remove(iTag);
        }


        #region TagMethods

        // ##### Tag Related Methods #####


        // returns an empty list if none are registered

        /// <summary>
        ///     Get All References to objects that are tagged with a Tag of the same Type as the given smartTag
        /// </summary>
        /// <param name="smartTag">A Tag that is defined as an Enum</param>
        /// <typeparam name="TEnum">Type of the Tag</typeparam>
        /// <returns>A list of References to Tagged objects</returns>
        public List<ITag> GetObjectsWithTagType<TEnum>(TEnum smartTag) where TEnum : Enum
        {
            var tagGroup = typeof(TEnum).ToString();
            return Tags.ContainsKey(tagGroup) ? Tags[tagGroup] : new List<ITag>();
        }

        /// <summary>
        ///     Get All References to objects that are tagged with the given smartTag
        /// </summary>
        /// <param name="smartTag">A Tag that is defined as an Enum</param>
        /// <typeparam name="TEnum">Type of the Tag</typeparam>
        /// <returns>A list of References to Tagged objects</returns>
        public List<ITag> GetObjectsWithTag<TEnum>(TEnum smartTag) where TEnum : Enum
        {
            return GetObjectsWithTagType(smartTag).FindAll(iTag => iTag.GetTag().Equals(smartTag));
        }

        /// <summary>
        ///     Get a single Reference to an object that is tagged with the given smartTag
        /// </summary>
        /// <param name="smartTag">A Tag that is defined as an Enum</param>
        /// <typeparam name="TEnum">Type of the Tag</typeparam>
        /// <returns>One Reference of a tagged Object</returns>
        public ITag GetObjectWithTag<TEnum>(TEnum smartTag) where TEnum : Enum
        {
            return GetObjectsWithTagType(smartTag).Find(iTag => iTag.GetTag().Equals(smartTag));
        }


        /// <summary>
        ///     Get a List of all tags that two objects have in common.
        /// </summary>
        /// <param name="o1">First Object</param>
        /// <param name="o2">Second Object</param>
        /// <returns>List of all Tags that appear on both Objects</returns>
        public List<Enum> GetCommonTags(GameObject o1, GameObject o2)
        {
            var o1Tags = o1.GetComponents<ITag>();
            var o2Tags = o2.GetComponents<ITag>();
            var matchingTags = new List<Enum>();
            foreach (var o1Tag in o1Tags)
            foreach (var o2Tag in o2Tags)
                if (o2Tag.Equals(o1Tag))
                    matchingTags.Add(o2Tag.GetTag());

            return matchingTags;
        }

        /// <summary>
        ///     Evaluate if the given Object is tagged with tag
        /// </summary>
        /// <param name="o">Object to check on for tag</param>
        /// <param name="smartTag">Tag to check for on o </param>
        /// <typeparam name="TEnum">Enum Tag Type</typeparam>
        /// <returns>true if there is an ITag on o that has tag selected as tag</returns>
        public bool CompareTag<TEnum>(GameObject o, TEnum smartTag) where TEnum : Enum
        {
            return o.GetComponents<ITag>().Any(oTag => oTag.GetTag().Equals(smartTag));
        }

        #endregion
    }
}