using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Extensions.Options;
using PCRE;

namespace Fun.SimpleGame.Configurations.Enemy
{
    public class EnemyConfigParser //: IEnemyConfigParser
    {
        private static readonly PcreRegex BuildTimeRegex = new("^(?:GT:(?<value>\\d+))$", PcreOptions.Compiled);

        private static readonly PcreRegex PositionRegex = new("^(?:Value:(?<value>-?\\d+))$", PcreOptions.Compiled);

        //public EnemyConfigParser(IOptions<>)
        //{
            
        //}

        public static Models.EnemyConfig Parse(string fileName)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
            var nodeList = xmlDoc.SelectNodes("/Config/Enemy | /Config/Group");

            var enemyList = new List<Models.EnemySpawn>();
            var enemyGroupList = new List<Models.EnemyGroup>();

            if (nodeList != null)
            {
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name == "Enemy")
                    {
                        enemyList.Add(new Models.EnemySpawn
                        {
                            BuildTime = ParseBuildTime(node),
                            Role = ParseRole(node),
                            Health = ParseHealth(node),
                            Position = ParsePositionFromElement(node),
                            Active = ParseActive(node)
                        });
                    }
                    else
                    {
                        if (node.ChildNodes.Count == 0)
                        {
                            throw new ArgumentException("Invalid 'Group' value. No enemies defined.");
                        }

                        var childEnemies = new List<Models.EnemySpawn>(5);
                        foreach (XmlNode enemyNode in node.ChildNodes)
                        {
                            if (enemyNode.Name != "Enemy")
                            {
                                throw new ArgumentException("Invalid 'Group' value.");
                            }

                            childEnemies.Add(new Models.EnemySpawn
                            {
                                BuildTime = ParseBuildTime(node),
                                Position = ParsePositionFromAttribute(node),
                                Role = ParseRole(enemyNode),
                                Health = ParseHealth(enemyNode),
                                Active = ParseActive(enemyNode)
                            });
                        }

                        enemyGroupList.Add(new Models.EnemyGroup(childEnemies.ToArray()));
                    }
                }
            }

            return new Models.EnemyConfig
            {
                EnemySpawns = enemyList
                    .OrderByDescending(i => i.BuildTime.StartImmediately)
                    .ThenBy(i => i.BuildTime.TriggerPoint)
                    .ToList(),
                EnemyGroups = enemyGroupList
                    .OrderByDescending(i => i.EnemySpawns[0].BuildTime.StartImmediately)
                    .ThenBy(i => i.EnemySpawns[0].BuildTime.TriggerPoint)
                    .ToList()
            };
        }

        private static Models.SpawnTiming ParseBuildTime(XmlNode node)
        {
            var buildTimeValue = GetAttributeValue(node, "BuildTime");
            if (buildTimeValue == "AtBegin")
            {
                return new Models.SpawnTiming(true);
            }

            var match = BuildTimeRegex.Match(buildTimeValue);
            if (match.Success)
            {
                var value = match.Groups["value"].Value;
                if (!int.TryParse(value, out var triggerPoint) || triggerPoint == 0)
                {
                    throw new ArgumentException("Invalid 'BuildTime' value.");
                }

                return new Models.SpawnTiming(triggerPoint);
            }

            throw new ArgumentException("Invalid 'BuildTime' value.");
        }

        private static Enums.EnemyRole ParseRole(XmlNode node)
        {
            var value = GetElementValue(node, "Role");
            if (!Enum.TryParse(typeof(Enums.EnemyRole), value, out var role))
            {
                throw new ArgumentException("Invalid 'Role' value.");
            }

            return (Enums.EnemyRole)role;
        }

        private static int ParseHealth(XmlNode node)
        {
            var value = GetElementValue(node, "Health");
            if (!int.TryParse(value, out var health) || health < 1)
            {
                throw new ArgumentException("Invalid 'Health' value.");
            }

            return health;
        }

        private static Models.SpawnLocation ParsePositionFromAttribute(XmlNode node)
        {
            return ParsePosition(GetAttributeValue(node, "Position"));
        }

        private static Models.SpawnLocation ParsePositionFromElement(XmlNode node)
        {
            return ParsePosition(GetElementValue(node, "Position"));
        }

        private static Models.SpawnLocation ParsePosition(string position)
        {
            var match = PositionRegex.Match(position);
            if (match.Success)
            {
                var value = match.Groups["value"].Value;
                if (!int.TryParse(value, out var offset))
                {
                    throw new ArgumentException("Invalid 'Position' value.");
                }

                return new Models.SpawnLocation(offset);
            }

            if (Enum.TryParse(typeof(Enums.SpawnArea), position, out var area))
            {
                return new Models.SpawnLocation((Enums.SpawnArea)area);
            }

            throw new ArgumentException("Invalid 'Position' value.");
        }

        private static bool ParseActive(XmlNode node)
        {
            var value = GetAttributeValue(node, "Active");
            if (!bool.TryParse(value, out var active))
            {
                throw new ArgumentException("Invalid 'Active' value.");
            }

            return active;
        }

        private static string GetAttributeValue(XmlNode node, string attributeName)
        {
            var value = node.Attributes?[attributeName]?.Value;
            if (value == null)
            {
                throw new ArgumentNullException(nameof(attributeName), $"Attribute '{attributeName}' is missing.");
            }

            return value;
        }

        private static string GetElementValue(XmlNode node, string elementName)
        {
            var value = node[elementName]?.InnerText;
            if (value == null)
            {
                throw new ArgumentNullException(nameof(elementName), $"Element '{elementName}' is missing.");
            }

            return value;
        }
    }
}