using System.Collections.Generic;
using System.Linq;

namespace Assets.Xyz.Scripts
{
    public class SpawnScheduleGenerator
    {
        public SpawnScheduleGenerator()
        {
            
        }

        public List<DifficultyEvent> GetSteppedSchedule()
        {
            return new List<DifficultyEvent>
            {
                new DifficultyEvent(10, "Security has arrived!", DifficultyEvent.Type.AddChasers),
                new DifficultyEvent(10, "", DifficultyEvent.Type.AddPushers),
                new DifficultyEvent(30, "Security has increased.", DifficultyEvent.Type.AddChasers),
                new DifficultyEvent(30, "", DifficultyEvent.Type.AddPushers),
                new DifficultyEvent(60, "We have reached max security!", DifficultyEvent.Type.AddChasers),
                //new DifficultyEvent(90, "", DifficultyEvent.Type.AddChasers),
            };
        }

        public List<DifficultyEvent> GetGradualSchedule(int start, int end, int amount)
        {
            var result = new List<DifficultyEvent>();
            var spawnTimes = GenerateSpawnTimes(start, end, amount);

            foreach (var spawnTime in spawnTimes)
            {
                string message = "";
                if (result.Count == 0)
                {
                    message = "Security has arrived!";
                }
                else if (spawnTime == spawnTimes.Last())
                {
                    message = "Max security has been reached!!!";
                }

                result.Add(new DifficultyEvent(spawnTime, message, DifficultyEvent.Type.AddChasers));
            }

            return result;
        }

        public List<float> GenerateSpawnTimes(int start, int end, int count)
        {
            var result = new List<float>();

            if (count <= 0)
            {
                return result;
            }

            float duration = end - start;
            float interval = (duration / count);
            float currentTime = start;
            while ((end - currentTime) > .01f)
            {
                result.Add(currentTime);
                currentTime += interval;
            }

            return result;
        }
    }
}
