namespace LandConquestDB.Models
{
    public sealed class MapModel
    {
        public static int[] CenterOfLand(int land_id)
        {
            switch (land_id)
            {
                case 1:
                    {
                        int m1 = 155;
                        int m2 = 83;
                        return new int[] { m1, m2 };
                    }
                case 2:
                    {
                        int m1 = 132;
                        int m2 = 87;
                        return new int[] { m1, m2 };
                    }
                case 3:
                    {
                        int m1 = 35;
                        int m2 = 67;
                        return new int[] { m1, m2 };
                    }
                case 4:
                    {
                        int m1 = 90;
                        int m2 = 70;
                        return new int[] { m1, m2 };
                    }
                case 5:
                    {
                        int m1 = 157;
                        int m2 = 45;
                        return new int[] { m1, m2 };
                    }
                case 6:
                    {
                        int m1 = 205;
                        int m2 = 70;
                        return new int[] { m1, m2 };
                    }
                case 7:
                    {
                        int m1 = 215;
                        int m2 = 190;
                        return new int[] { m1, m2 };
                    }
                case 8:
                    {
                        int m1 = 95;
                        int m2 = 125;
                        return new int[] { m1, m2 };
                    }
                case 9:
                    {
                        int m1 = 165;
                        int m2 = 150;
                        return new int[] { m1, m2 };
                    }
                case 10:
                    {
                        int m1 = 147;
                        int m2 = 107;
                        return new int[] { m1, m2 };
                    }
                case 11:
                    {
                        int m1 = 230;
                        int m2 = 120;
                        return new int[] { m1, m2 };
                    }

            }
            return new int[] { 1488, 1488 }; ;
        }
    }
}
