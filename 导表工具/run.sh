#!/bin/zsh
cd /Users/guoxuan/Game/SSRPG/导表工具

dotnet Luban.ClientServer/Luban.ClientServer.dll -j cfg -- \
    -d Defines/__root__.xml \
    --input_data_dir Excels \
    --output_code_dir ../Assets/GameMain/Scripts/Gen \
    --output_data_dir ../Assets/GameMain/GameData/CfgData \
    --gen_types code_cs_bin,data_bin \
    --data_file_extension bytes \
    -s all \