
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DruidLightning : MonoBehaviour {

	// 1. The Druid's lightning is an attack which finds a target bloon within 36 range.
	// 2. The lightning hits the bloon target instantly, then splits after 0.05 seconds (3 frames).
	// 3. Every time a lightning splits, it finds two more bloon targets within a range of 86.
	// 4. The main lightning splits up to 4 times. eg 1 becomes 2, 2 becomes 4, 4 becomes 8, 8 becomes 16. Up to 31 targets!

	// Additional notes:
	// - Lightning cannot strike the same bloon more than once.
	// - Bloons game logic happens in the 2d space, so we should be ignoring the z space.

	private int LightningDamage = 2;
	private int LightningSplits = 2;
	private int LightningSplitRange = 86;

	private float split1At = 0.05f;
	private float split2At = 0.10f;
	private float split3At = 0.15f;
	private float split4At = 0.20f;

	private bool foundTarget = false;
	private bool hasSplit1 = false;
	private bool hasSplit2 = false;
	private bool hasSplit3 = false;
	private bool hasSplit4 = false;

	private float AttackStartedAt = -1f;
	private float CurrentTimeline = -1f;

	private Bloon targetBloon;
	private List<Vector3> split1Origins;
	private List<Vector3> split2Origins;
	private List<Vector3> split3Origins;
	private List<Vector3> split4Origins;

	private Tower tower;

	public class Vector3 {
		public float x;
		public float y;
		public float z;
	}

	public class Bloon {
		public Vector3 position;
		public int health;
	}

	public class Tower {
		public float range;
		public Vector3 position;
	}

	// This function is called externally and needs to exist.
	public void Initialise(Tower tower) {
		this.tower = tower;
	}

	// This function is called externally and needs to exist.
	public void UpdateLightning(float delta, List<Bloon> allBloons) {
		if (AttackStartedAt <= 0f) {
			return;
		}

		if (! foundTarget) {
			return;
		}

		CurrentTimeline += delta;

		if (CurrentTimeline >= split1At && !hasSplit1) {
			// Split 1
			foreach (var origin in split1Origins) {
				List<Bloon> bloonsInRange = new List<Bloon>();
				foreach (var bloon in allBloons.Where(b => b.health > 0)) {
					var distanceVector = new Vector3();
					distanceVector.x = bloon.position.x - origin.x;
					distanceVector.y = bloon.position.y - origin.y;
					distanceVector.z = bloon.position.z - origin.z;

					var distanceSquared = distanceVector.x * distanceVector.x + distanceVector.y * distanceVector.y + distanceVector.z * distanceVector.z;
					var distance = Mathf.Sqrt(distanceSquared);

					if (split2Origins.Contains(bloon.position)) {
						continue;
					}

					if (distance <= LightningSplitRange) {
						bloonsInRange.Add(bloon);
					}
				}

				foreach (var bloon in bloonsInRange) {
					bloon.health -= LightningDamage;

					split2Origins.Add(bloon.position);

					if (split2Origins.Count >= LightningSplits) {
						break;
					}
				}
			}

			hasSplit1 = true;
		}

		if (CurrentTimeline >= split2At && !hasSplit2) {
			// Split 2
			foreach (var origin in split2Origins) {
				List<Bloon> bloonsInRange = new List<Bloon>();
				foreach (var bloon in allBloons.Where(b => b.health > 0)) {
					var distanceVector = new Vector3();
					distanceVector.x = bloon.position.x - origin.x;
					distanceVector.y = bloon.position.y - origin.y;
					distanceVector.z = bloon.position.z - origin.z;

					var distanceSquared = distanceVector.x * distanceVector.x + distanceVector.y * distanceVector.y + distanceVector.z * distanceVector.z;
					var distance = Mathf.Sqrt(distanceSquared);

					if (split3Origins.Contains(bloon.position)) {
						continue;
					}

					if (distance <= LightningSplitRange) {
						bloonsInRange.Add(bloon);
					}
				}

				foreach (var bloon in bloonsInRange) {
					bloon.health -= LightningDamage;

					split3Origins.Add(bloon.position);

					if (split3Origins.Count >= LightningSplits * LightningSplits) {
						break;
					}
				}
			}
			hasSplit2 = true;
		}

		if (CurrentTimeline >= split3At && !hasSplit3) {
			// Split 3
			foreach (var origin in split3Origins) {
				List<Bloon> bloonsInRange = new List<Bloon>();
				foreach (var bloon in allBloons.Where(b => b.health > 0)) {
					var distanceVector = new Vector3();
					distanceVector.x = bloon.position.x - origin.x;
					distanceVector.y = bloon.position.y - origin.y;
					distanceVector.z = bloon.position.z - origin.z;

					var distanceSquared = distanceVector.x * distanceVector.x + distanceVector.y * distanceVector.y + distanceVector.z * distanceVector.z;
					var distance = Mathf.Sqrt(distanceSquared);

					if (split4Origins.Contains(bloon.position)) {
						continue;
					}

					if (distance <= LightningSplitRange) {
						bloonsInRange.Add(bloon);
					}
				}

				foreach (var bloon in bloonsInRange) {
					bloon.health -= LightningDamage;

					split4Origins.Add(bloon.position);

					if (split4Origins.Count >= LightningSplits * LightningSplits * LightningSplits) {
						break;
					}
				}
			}
			hasSplit3 = true;
		}

		if (CurrentTimeline >= split4At && !hasSplit4) {
			// Split 4
			foreach (var origin in split4Origins) {
				List<Bloon> bloonsInRange = new List<Bloon>();
				foreach (var bloon in allBloons.Where(b => b.health > 0)) {
					var distanceVector = new Vector3();
					distanceVector.x = bloon.position.x - origin.x;
					distanceVector.y = bloon.position.y - origin.y;
					distanceVector.z = bloon.position.z - origin.z;

					var distanceSquared = distanceVector.x * distanceVector.x + distanceVector.y * distanceVector.y + distanceVector.z * distanceVector.z;
					var distance = Mathf.Sqrt(distanceSquared);

					if (distance <= LightningSplitRange) {
						bloonsInRange.Add(bloon);
					}
				}

				foreach (var bloon in bloonsInRange) {
					bloon.health -= LightningDamage;

					if (split4Origins.Count >= LightningSplits * LightningSplits * LightningSplits * LightningSplits) {
						break;
					}
				}
			}

			hasSplit4 = true;
		}
	}

	// This function is called externally and needs to exist.
	public void CheckTowerAttack(float currentTime, List<Bloon> allBloons) {
		if (foundTarget) {
			return;
		}

		List<Bloon> bloonsInRange = new List<Bloon>();

		// Get bloons in range
		foreach (var bloon in allBloons.Where(b => b.health > 0)) {
			var distanceVector = new Vector3();
			distanceVector.x = bloon.position.x - tower.position.x;
			distanceVector.y = bloon.position.y - tower.position.y;
			distanceVector.z = bloon.position.z - tower.position.z;

			var distanceSquared = distanceVector.x * distanceVector.x + distanceVector.y * distanceVector.y + distanceVector.z * distanceVector.z;
			var distance = Mathf.Sqrt(distanceSquared);

			if (distance <= tower.range) {
				bloonsInRange.Add(bloon);
			}
		}

		var foundBloon = false;
		foreach (var bloon in bloonsInRange) {
			foundBloon = true;
			targetBloon = bloon;
		}

		if (!foundBloon) {
			return;
		}

		if (foundBloon) {
			foundTarget = true;
		}

		targetBloon.health -= LightningDamage;

		AttackStartedAt = currentTime;

		split1Origins.Add(targetBloon.position);
	}
}