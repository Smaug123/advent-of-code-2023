{
  description = "Advent of Code 2023";
  inputs = {
    flake-utils.url = "github:numtide/flake-utils";
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
  };

  outputs = {
    self,
    nixpkgs,
    flake-utils,
  }:
    flake-utils.lib.eachDefaultSystem (
      system: let
        pkgs = nixpkgs.legacyPackages.${system};
      in
        # Conditionally include Swift and Apple SDK for Darwin systems
        let
          darwinDeps =
            if system == "x86_64-darwin" || system == "aarch64-darwin"
            then [
              pkgs.swift
              pkgs.darwin.apple_sdk.frameworks.Foundation
              pkgs.darwin.apple_sdk.frameworks.CryptoKit
              pkgs.darwin.apple_sdk.frameworks.GSS
            ]
            else [];
        in {
          devShells = {
            default = pkgs.mkShell {
              buildInputs = with pkgs;
                [
                  (with dotnetCorePackages;
                    combinePackages [
                      dotnet-sdk_8
                      dotnetPackages.Nuget
                    ])
                ]
                ++ darwinDeps
                ++ [pkgs.zlib pkgs.zlib.dev pkgs.openssl pkgs.icu pkgs.alejandra];
            };
          };
        }
    );
}
